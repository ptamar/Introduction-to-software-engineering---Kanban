using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Text.Json;
using IntroSE.Kanban.Backend.Buissnes_Layer;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// A class for grading your work <boardService>ONLY</boardService>. The methods are not using good SE practices and you should <boardService>NOT</boardService> infer any insight on how to write the service layer/business layer. 
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
    ///             <para>If the function does not return a value or an exception has occorred, then the field should be either null or undefined.</para>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>ErrorMessage</term>
    ///         <description>If an exception has occorred, then this field will contain a string of the error message. Otherwise, the field will be null or undefined.</description>
    ///     </item>
    /// </list>
    /// </para>
    /// <para>
    /// An empty response is a response that both fields are either null or undefined.
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
    public class GradingService
    {
        private UserController userController;
        private BoardService boardService;
        public UserService userService;
        private TaskService taskService;
        private const int BacklogState = 0;
        private const int InProgressState = 1;
        private const int Done = 2;
        public GradingService()
        {
            this.userController = new UserController();
            this.boardService = new BoardService(this.userController);
            this.userService = new UserService(this.userController);
            this.taskService = new TaskService(this.boardService.boardController);
        }


        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Register(string email, string password)
        {
            return userService.CreateUser(email, password);


            // try
            // {
            //     userController.CreateUser(email, password);
            //     Response response = new Response(null, null);
            //     return ToJson.toJson(response);
            // }
            // catch (Exception e)
            // {
            //     Response response = new Response(e.Message, null);
            //     return ToJson.toJson(response);
            // }
        }


        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Login(string email, string password)
        {
            return userService.Login(email, password);
        }

        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Logout(string email)
        {
            return userService.logout(email);
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                boardService.LimitColumn(email, boardName, columnOrdinal, limit);
                Response response = new Response(null);
                return ToJson.toJson(response);
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                return boardService.GetColumnLimit(email, boardName, columnOrdinal);
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }

        }


        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                string colName = boardService.GetColumnName(email, boardName, columnOrdinal);
                Response response = new Response(null, colName);
                return ToJson.toJson(response);

            }
            catch (Exception e)
            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }

        }


        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                string r = boardService.AddTask(email, boardName, title, description, dueDate);
                Response response = new Response(null, null);
                return ToJson.toJson(response);
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }



        }


        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                if (boardService.boardController.GetTask(email, boardName, taskId, columnOrdinal).GetState() ==
                    columnOrdinal)
                {
                    return taskService.EditDueDate(email, boardName, taskId, dueDate);
                }
                else
                {
                    Response response = new Response("Not the right column number", null);
                    return ToJson.toJson(response);
                }
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message);
                return ToJson.toJson(response);
            }
            
        }




        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                if (boardService.boardController.GetTask(email, boardName, taskId, columnOrdinal).GetState() ==
                    columnOrdinal)
                {
                    try
                    {
                        taskService.EditTitle(email, boardName, taskId, title);
                        Response response = new Response(null);
                        return ToJson.toJson(response);
                    }
                    catch (Exception e)
                    {
                        Response response = new Response(e.Message, null);
                        return ToJson.toJson(response);
                    }
                }
                else
                {
                    Response response = new Response("Not the right column number", null);
                    return ToJson.toJson(response);
                }

            }
            catch (Exception e)
            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }
        }


        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId,
            string description)
        {
            try
            {
                if (boardService.boardController.GetTask(email, boardName, taskId, columnOrdinal).GetState() ==
                    columnOrdinal)
                {
                    try
                    {

                        taskService.EditDescription(email, boardName, taskId, description);
                        Response response = new Response(null);
                        return ToJson.toJson(response);
                        

                        // try
                        // {
                        //     taskService.EditDescription(email, boardName, taskId, description);
                        //     Response response = new Response(null);
                        //     return ToJson.toJson(response); ;
                        // }
                        // catch (Exception e)
                        // {
                        //     Response response = new Response(e.Message, null);
                        //     return ToJson.toJson(response);
                        // }

                    }
                    catch (Exception e)
                    {
                        Response response = new Response(e.Message, null);
                        return ToJson.toJson(response);
                    }
                }
                else
                {
                    Response response = new Response("Not the right column number");
                    return ToJson.toJson(response);
                }

            }
            catch (Exception e)
            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }
        }


        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            
                return boardService.NextState(email, boardName, columnOrdinal, taskId);
        }


        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            try
            {
                return boardService.GetColum(email, boardName, columnOrdinal);
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }

        }


        /// <summary>
        /// This method adds a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddBoard(string email, string name)
        {
            try
            {
                boardService.boardController.CreateBoard(email, name);
                Response response = new Response(null);
                return ToJson.toJson(response);
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }
        }


        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string RemoveBoard(string email, string name)
        {
            try
            {
                boardService.DeleteBoard(name, email);
                Response response = new Response(null);
                return ToJson.toJson(response);
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }
        }


        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string InProgressTasks(string email)
        {
            try
            {
                return boardService.InProgress(email);

            }
            catch (Exception e)
            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }
        }

        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetUserBoards(string email)
        {
            try
            {
                Response r = new Response(boardService.boardController.GetUserBList(email));
                return ToJson.toJson(r);
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string JoinBoard(string email, int boardID)
        {
            return boardService.JoinBoard(boardID, email);
             
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LeaveBoard(string email, int boardID)
        {
            try
            {
                boardService.LeaveBoard(boardID, email);
                Response response = new Response(null);
                return ToJson.toJson(response);
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
                //List<Task> t = boardService.InProgress(email);
                //Response response = new Response(null, email);
                //return ToJson.toJson(response);
                boardService.AssignTask(emailAssignee, boardName, columnOrdinal, email, taskID);
                Response response = new Response(null);
                return ToJson.toJson(response);
            }
            catch (Exception e)

            {
                Response response = new Response(e.Message, null);
                return ToJson.toJson(response);
            }
        }

        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LoadData()
        {
            try
            {
                this.boardService.LoadData();
                this.userService.LoadData();
                Response r = new Response(null);
                return ToJson.toJson(r);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Response r = new Response(null);
                return ToJson.toJson(r);
            }

        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteData()
        {
            try
            {
                userService.DeleteAllDataVoid();
                boardService.DeleteAllData();
                Response r = new Response(null);
                return ToJson.toJson(r);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Response r = new Response(e.Message);
                return ToJson.toJson(r);
            }

            // Probably need to add deletion of all tasks through TaskService

        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            return boardService.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
        }
    }
}


