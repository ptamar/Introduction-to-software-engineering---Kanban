using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using Newtonsoft.Json;

namespace Frontend.Model
{
    internal class UserModel
    {
        private ServiceFactory serviceFactory;
        private UserService userService;

        public UserModel()
        {
            this.serviceFactory = ServiceFactory.getServiceFactrory();
            this.userService = serviceFactory.userService;
        }

        /// <summary>
        /// This login the user.
        /// </summary>
        /// <param name="username">The email address of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>return if the user logged in. </returns>
        public string Login(string username, string password)
        {
            return userService.login(username, password);
        }
        /// <summary>
        /// This logout the user.
        /// </summary>
        /// <param name="username">The email address of the user.</param>
        /// <returns>void. </returns>
        public void Logout(string username)
        {
            userService.logout(username);
        }
        /// <summary>
        /// This Register the user.
        /// </summary>
        /// <param name="username">The email address of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>bool. </returns>
        public Boolean Register(string Username, string Password)
        {
            string userCreation = userService.Create_User(Username, Password);
            if (userCreation == "{}")
            {
                return true;
            }
            else
            {
                throw new ArgumentException(userCreation);
            }
        }
    }
}
