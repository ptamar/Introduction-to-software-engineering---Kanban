using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class BoardUsersDTO
    {
        internal int BoardID;
        internal string userName;

        internal BoardUsersDTO(int boardID, string username)
        {
            BoardID = boardID;
            userName = username;
        }


    }
}
