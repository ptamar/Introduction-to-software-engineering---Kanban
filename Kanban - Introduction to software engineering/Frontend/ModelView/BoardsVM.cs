using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Model;

namespace Frontend.ModelView
{
    internal class BoardsVM
    {
        private BoardsModel _boardsModel;
        private string _email;
        public BoardsVM(string email)
        {
            _boardsModel = new BoardsModel(email);
            _email = email;
        }
        /// <summary>
        /// This method get all the name of the boards of the user.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>return Dictionary with the name of the boards . </returns>
        public Dictionary<int, string> GetBoards(string email)
        {
            return _boardsModel.GetBoardNames(email);
        }
    }
}
