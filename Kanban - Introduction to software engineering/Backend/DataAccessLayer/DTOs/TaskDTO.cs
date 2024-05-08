using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public class TaskDTO
    {
        public int taskID;
        public int boardID;
        public string assignee;
        public string status;
        public string title;
        public string description;
        public string dueDate;
        public string creationTime;

        /// <summary>
        /// Constructor for a task DTO.
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="boardID"></param>
        /// <param name="assignee"></param>
        /// <param name="status"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <param name="creationTime"></param>
        public TaskDTO(int taskID, int boardID, string assignee, string status, string title, string description, string dueDate, string creationTime)
        {
            this.taskID = taskID;
            this.boardID = boardID;
            this.assignee = assignee;
            this.status = status;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.creationTime = creationTime;
        }

        public int GetState()
        {
            if (status == "backlog")
            {
                return 0;
            }
            else if (status == "in progress")
            {
                return 1;
            }
            else if (status == "Done")
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }



        // public void EditTitle(string newTitle)
        // {
        //     Title = newTitle;
        // }
        // public void SetState(int state)
        // {
        //     State = state;
        // }
        //
        // public void EditDescription(string newDescription)
        // {
        //     Description = newDescription;
        // }
        //
        // public void EditDueDate(DateTime newDueDate)
        // {
        //     DueDate = newDueDate;
        // }
        //
        // public void EditAssignee(string userEmail)
        // {
        //     Assignee = userEmail;
        // }
    }
}