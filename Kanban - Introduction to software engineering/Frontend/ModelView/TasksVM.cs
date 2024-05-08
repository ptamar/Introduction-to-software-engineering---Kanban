using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Frontend.Model;
using IntroSE.Kanban.Backend.Buissnes_Layer;

namespace Frontend.ModelView
{
    internal class TasksVM
    {
        private TasksModel tasksModel;
        public TasksVM()
        {
            tasksModel = new TasksModel("email", "boardName");
        }
        /// <summary>
        /// This method get the Column from the right board .
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The asked column.</param>
        /// <returns>return a list with the Column from the right board . </returns>
        public List<string> GetColumn(string email, string boardName, int colId)
        {

            return tasksModel.GetColumn(email, boardName, colId);
        }
     
    }
}
