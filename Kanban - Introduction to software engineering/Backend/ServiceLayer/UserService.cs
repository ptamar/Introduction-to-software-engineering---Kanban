using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buissnes_Layer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// A class for calling the UserService class in the BussinessLayer.
    /// <para>
    /// Each of the class' methods should return a JSON string with the following structure (see <see cref="System.Text.Json"/>):
    /// <code>
    /// {
    ///     "ErrorMessage": &lt;string&gt;,
    ///     "ReturnValue": &lt;object&gt;
    /// }
    /// </code>
    /// Where:
    /// <list type="bullet">
    ///     <item>
    ///         <term>ReturnValue</term>
    ///         <description>
    ///             The return value of the function.
    ///             <para>
    ///                 The value may be either a <paramref name="primitive"/>, a <paramref name="Task"/>, or an array of of them. See below for the definition of <paramref name="Task"/>.
    ///             </para>
    ///             <para>If the function does not return a value or an exception has occorred, then the field is undefined.</para>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>ErrorMessage</term>
    ///         <description>If an exception has occurred, then this field will contain a string of the error message.</description>
    ///     </item>
    /// </list>
    /// </para>
    /// <para>
    /// The structure of the JSON of a TaskService, is:
    /// <code>
    /// {
    ///     "Id": &lt;int&gt;,
    ///     "CreationTime": &lt;DateTime&gt;,
    ///     "Title": &lt;string&gt;,
    ///     "Description": &lt;string&gt;,
    ///     "DueDate": &lt;DateTime&gt;
    /// }
    /// </code>
    /// </para>
    /// </summary>
    public class UserService
    {
        public readonly UserController userController;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>Response with a createUser task, unless user already exists.</returns>

        public UserService(UserController UC)
        {
            this.userController = UC; 

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
        }
        public string CreateUser(string email, string password)

        {
            try
            {
                userController.CreateUser(email, password);
                userController.Login(email, password);

                String msg = String.Format("User created! email = {0}", email);
                log.Info(msg);
                
                Response response = new Response(null, null);
                // If successful returns user object in JSON
                return ToJson.toJson(response);
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message);
                return ToJson.toJson(response);

            }
        }
        public string Create_User(string email, string password)

        {
            try
            {
                userController.CreateUser(email, password);
                userController.Login(email, password);

                String msg = String.Format("User created! email = {0}", email);
                log.Info(msg);

                Response response = new Response(null, null);
                // If successful returns user object in JSON
                return ToJson.toJson(response);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        /// <summary>
        ///  This method changes the password of a given user.
        /// Method is never referenced. Check if implemented in DAL.
        /// </summary>
        /// <param name="username">The email address of the user.</param>
        /// <param name="oldP">The old password of the user. Must match with existing password in database.</param>
        /// <param name="newP">The new password of the user. Must match with password rules.</param>
        /// <returns> Response with changePassword task, unless an error occurs.</returns>
        public string ChangePassword(string username, string oldP, string newP)
        {
            try
            {
                // TODO: Make ServiceLayer unfamiliar
                userController.GetUser(username).ChangePassword(oldP, newP);
                String msg = String.Format("Password changed successfully for user - {0}", username);
                log.Info(msg);
                Response response = new Response(null);
                return ToJson.toJson(response); 
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message);
                return ToJson.toJson(response); 
            }
        }


        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="username">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns> Response with user email, unless an error occurs</returns>
        public string Login(string username, string password)
        {
            try
            {
                userController.Login(username, password);
                String msg = String.Format("Login successful for user - {0}", username);
                log.Info(msg);
                Response response = new Response(null, username);
                return ToJson.toJson(response);
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message);
                return ToJson.toJson(response);
            }

            ;
        }
        public string login(string username, string password)
        {
            try
            {
                userController.Login(username, password);
                String msg = String.Format("Login successful for user - {0}", username);
                log.Info(msg);
                return username;
            }
            catch (Exception e)
            {
                return e.Message;
            } ;
        }


        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="username">The email of the user to log out</param>
        /// <returns>The string "{}", unless an error occurs </returns>
        public string logout(string username)
        {
            try
            {
                userController.Logout(username);
                String msg = String.Format("Logout successful for user - {0}", username);
                log.Info(msg);
                Response response = new Response(null);
                return ToJson.toJson(response);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                Response response = new Response(e.Message);

                return ToJson.toJson(response);
            }

        }

        public string LoadData()
        {
            try
            {
                this.userController.LoadUsers();
                String msg = String.Format("Load Data successful in UserService.");
                log.Info(msg);
                Response response = new Response(null, null);
                return ToJson.toJson(response);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                Response response = new Response(e.Message);
                return ToJson.toJson(response);
            }
            
        }

        public string DeleteAllData()
        {
            try
            {
                this.userController.DeleteAllData();
                String msg = String.Format("Load Data successful in UserService.");
                log.Info(msg);
                Response response = new Response(null, null);
                return ToJson.toJson(response);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                Response response = new Response(e.Message);
                return ToJson.toJson(response);
            }
            
        }

        public void DeleteAllDataVoid()
        {
            this.userController.DeleteAllData();
            // try
            // {
            //     this.userController.DeleteAllData();
            //     String msg = String.Format("Load Data successful in UserService.");
            //     log.Info(msg);
            //     Response response = new Response(null, null);
            //     return ToJson.toJson(response);
            // }
            // catch (Exception e)
            // {
            //     log.Warn(e.Message);
            //     Response response = new Response(e.Message);
            //     return ToJson.toJson(response);
            // }

        }

    }
}
