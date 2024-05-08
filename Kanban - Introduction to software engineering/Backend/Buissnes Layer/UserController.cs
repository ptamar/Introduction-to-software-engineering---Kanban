using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer.Mappers;

namespace IntroSE.Kanban.Backend.Buissnes_Layer
{
    /// <summary>
    /// A class to instantiate users and control their logic. As of the end of milestone 2 -
    /// BusinessLayer classes should call the IsLoggedIn method.
    /// ServiceLayer classes should call the CreateUser, Login and Logout methods.
    /// </summary>
    public class UserController
    {

        private Dictionary<string, User> users;
        private List<string> loggedIn;
        private UserDTOMapper userDtoMapper;
        private const int MinPasswordLength = 6;
        private const int MaxPasswordLength = 20;
        /// <summary>
        /// Constructor for the class. Instantiates its private fields.
        /// The class must be instantiated in order to call its methods and functionality.
        /// </summary>
        public UserController()
        {
            this.users = new Dictionary<string, User>();
            this.loggedIn = new List<string>();
            this.userDtoMapper = new UserDTOMapper();
            // Do NOT Load Data!
            // LoadUsers(); - This is extremely bad - constructors cannot throw exceptions
        }

        /// <summary>
        /// Checks whether a string contains Hebrew characters. Should be used in conjunction with IsLegalEmail.
        /// </summary>
        private bool IsHebrew(string str)
        {
            string[] heb =
            {
                "א", "ב", "ג", "ד", "ה", "ו", "ז", "ח", "ט", "י", "כ", "ל", "מ", "נ", "ס", "ע", "פ", "צ", "ק", "ר", "ש",
                "ת", "ף", "ץ", "ך", "ן"
            };
            List<string> hebrew = new List<string>(heb);
            for (int i = 0; i < heb.Length; i++)
            {
                if (str.Contains(heb[i]))
                {
                    return true;
                }

            }

            return false;

        }

        /// <summary>
        /// Checks whether the an email address is valid using a regex.
        /// </summary>
        private bool IsValidEmail(string email){ 
            Regex regex = new Regex(
                @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
            
        Regex regex1 = new Regex(@"^(\w.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Regex regex2 = new Regex(@"^\w+([.-]?\w+)@\w+([.-]?\w+)(.\w{2,3})+$");
        var emailValidation = new EmailAddressAttribute();
        Match emailMatch = regex.Match(email);
        Match emailMatch1 = regex1.Match(email);
        Match emailMatch2 = regex2.Match(email);
        bool emailMatch3 = emailValidation.Match(email);
            return emailMatch1.Success || emailMatch2.Success || emailMatch3 || emailMatch.Success;


        }
    /// <summary>
    /// Checks whether the an email address is valid using a regex.
    /// </summary>
    bool IsValidEmail2(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Creates a user. A new user's email must not preexist in the system and must have a valid email and password.
        /// </summary>
        public void CreateUser(string email, string password)
        {
            string Email = email.ToLower();
            if (this.users != null)
            {
                if (UserExists(Email))
                {
                    throw new ArgumentException("Can't Register - User already exists");
                }
                if (!IsValidEmail(Email)||!IsValidEmail2(Email))
                {
                    throw new ArgumentException("Can't Register - Not a valid email address");
                }

                if (!IsLegalPassword(password))
                {
                    throw new ArgumentException("Can't Register - Illegal password. A legal password must be 6-20 characters" +
                                                " and must contain an Upper case, a lower case and a number");
                }
                else
                {
                    UserDTO userDto = userDtoMapper.CreateUser(Email, password);
                    User u = new User(userDto);
                    // User u = new User(Email, password);
                    users.Add(Email, u);
                }
            }
            else
            {
                if (!IsValidEmail(Email))
                {
                    throw new ArgumentException("Can't Register - not a valid email address");
                }
                if (!IsLegalPassword(password))
                {
                    throw new ArgumentException("Can't Register - Illegal password. A legal password must be 6-20 characters" +
                                                " and must contain an Upper case, a lower case and a number");
                }
                else
                {
                    UserDTO userDto = userDtoMapper.CreateUser(Email, password);
                    User u = new User(userDto);
                    users.Add(Email, u);
                    // User u = new User(Email, password);
                    users.Add(Email, u);
                }
                
            }
        }

        /// <summary>
        /// Deletes a user from the system.
        /// NOT implemented in DAL.
        /// </summary>
        public void DeleteUser(string email)
        {

            try
            {
                string Email = email.ToLower();
                users.Remove(Email);
            }
            catch (Exception){
                throw new ArgumentException("User does not exist");
            }
        }

        /// <summary>
        /// Checks whether a user exists in the system.
        /// </summary>
        private bool UserExists(string email)
        {
            string Email = email.ToLower();
            return users.ContainsKey(Email);
        }

        /// <summary>
        /// Returns a user object. Used for internal purposes.
        /// </summary>
        public User GetUser(string username)
        {
            string Email = username.ToLower();
            if (users.ContainsKey(Email))
            {
                return users[Email];
            }
            else
            {
                throw new ArgumentException("User does not exist");
            }

        }

        /// <summary>
        /// Checks whether a password is legal according to the system requirements.
        /// A legal password is 6-20 characters, contains at least 1 uppercase, 1 lower case and one number
        /// </summary>
        private bool IsLegalPassword(string password)
        {
            if (password.Length < MinPasswordLength)
            {
                return false;
            }
            if (password.Length > MaxPasswordLength )
            {
                return false;
            }
            if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsNumber)||IsHebrew(password))
            {
                return false;
            }

            if (!password.Any(char.IsAscii))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        private bool ValidatePassword(string email, string password)
        {
            string Email = email.ToLower();
            if (users.ContainsKey(Email))
            {
                return users[Email].ValidatePassword(password);
            }
            return false;
        }

        public void Login(string email, string password)
        {
            string Email = email.ToLower();
            if (!UserExists(Email))
            {
                throw new ArgumentException("User does not exist");
            }

            else if (!ValidatePassword(Email, password))
            {
                throw new ArgumentException("Wrong password");
            }

            // else if (loggedIn.Contains(Email))
            //
            // {
            //     throw new ArgumentException("User is already logged in");
            // }

            else
            {
                loggedIn.Add(Email);
            }
        }

        public void Logout(string Email)
        {
            string email = Email.ToLower();
            if (!UserExists(email))
            {
                throw new ArgumentException("User does not exist");
            }

            else if (!loggedIn.Contains(email))

            {
                throw new ArgumentException("User is already logged out");
            }

            else
            {
                loggedIn.Remove(email);
            }

        }

        public bool IsLoggedIn(string Email)
        {
            string email = Email.ToLower();
            // if (!UserExists(email))
            // {
            //     throw new ArgumentException("User does not exist");
            // }
            return loggedIn.Contains(email);
        }

        public void LoadUsers()
        {
            List<UserDTO> userDtos = userDtoMapper.LoadUsers();
            foreach (var u in userDtos)
            {
                User user = new User(u);
                users[user.username] = user;
                // Console.WriteLine($"User {user.username} loaded successfully!");
            }
            
        }

        public void DeleteAllData()
        {
            this.userDtoMapper.DeleteAllData();
            this.users.Clear();
            this.loggedIn.Clear();
        }
    }
}