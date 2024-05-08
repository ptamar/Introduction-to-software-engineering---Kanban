using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Buissnes_Layer;
using IntroSE.Kanban.Backend.ServiceLayer;
using Newtonsoft.Json;

namespace Frontend.Model
{
    

    internal class BoardsModel
    {
        private BoardService boardService;
        private ServiceFactory serviceFactory;
        private string _email;

        public BoardsModel(string email)
        {
            this.serviceFactory = ServiceFactory.getServiceFactrory();
            this.boardService = serviceFactory.boardService;
            this._email = email;
        }
        /// <summary>
        /// This method get all the name of the boards of the user.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>return Dictionary with the name of the boards . </returns>
        public Dictionary<int, string> GetBoardNames(string email)
        {
            return boardService.GetUserBoards(email);
        }
        
        

    }
}
