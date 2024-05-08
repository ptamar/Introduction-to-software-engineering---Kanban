using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class UserDTO
    {
        private string username;
        public string Username => username;
        private string password;
        public string Password => password;

        public UserDTO(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// Changes the password of a user DTO. Does not use logic, assumes the action is legal.
        /// </summary>
        /// <param name="NewPassword"></param>
        public void ChangePassword(string NewPassword)
        {
            password = NewPassword;
        }
    }
}
