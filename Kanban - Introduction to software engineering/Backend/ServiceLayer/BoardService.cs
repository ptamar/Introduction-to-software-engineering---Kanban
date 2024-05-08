using IntroSE.Kanban.Backend.Buissnes_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using log4net;
using log4net.Config;
using Task = IntroSE.Kanban.Backend.Buissnes_Layer.Task;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// boardController instance should be changed to private!!!
    /// </summary>
    public class BoardService
    {

        public BoardController boardController; // Should be Private!!!!!
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BoardService(UserController UC)
        {
            this.boardController = new BoardController(UC);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
        }

        /// <summary>
        /// This method creates a new board.
        /// </summary>
        /// <param name="name">The name of the board</param>
        /// <param name="userEmail">Email of the user. To connect between the new board to the user who made it.</param>
        /// <returns>Response with a command to create board, unless  an error occurs.</returns>
        public string CreateBoard(string name, string userEmail)
        {
            try
            {
                boardController.CreateBoard(userEmail, name);
                String msg = String.Format("Board Added Successfully! to email :{0}", userEmail);
                log.Info(msg);
                return "{}";
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                //RETURN BAD JASON
                Response r = new Response(e.Message, false);
                return r.BadJson();
            }

        }

        /// <summary>
        /// This method add new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>Response with user-email, unless an error occurs .</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                boardController.AddTaskB(email, boardName, title, description, dueDate);
                String msg = String.Format("task added Successfully! to board :{0}", boardName);
                log.Info(msg);
                Response r = new Response(null, email);
                return ToJson.toJson(r);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new Exception(e.Message);
            }


        }

        /// <summary>
        /// This method updates the state of the  task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>Response with a command to move the task state, unless doesn't exists a task with the same name.</returns>
        public string NextState(string email, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                boardController.NextStateB(email, boardName, columnOrdinal, taskId);
                String msg =
                    String.Format($"task changed state Successfully in BuissnesLayer! to state {columnOrdinal}");
                log.Info(msg);
                Response r = new Response(null);
                return ToJson.toJson(r);

            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                Response r = new Response(e.Message);
                return ToJson.toJson(r);
            }

        }

        /// <summary>
        /// This method add a new member to a board.
        /// </summary>
        /// <param name="userEmailOwner">Email of the user owner.</param>
        /// <param name="name">The name of the board</param>
        /// <param name="userEmail">Email of the user added.</param>
        /// <returns>An empty response, unless an error occurs.</returns>
        public string JoinBoard(int boardId, string userEmail)
        {
            try
            {
                boardController.joinBoard(boardId, userEmail);
                Response r = new Response(null, null);
                // String msg = String.Format("joined Board! userEmailJJoiner = {0} ", userEmail);
                // log.Info(msg);
                return ToJson.toJson(r);
            }
            catch (Exception e)
            {
                Response response = new Response(e.Message);
                return ToJson.toJson(response);
            }

        }

        /// <summary>
        /// This method remove a member from a board.
        /// </summary>
        /// <param name="boardId">The Id of the board</param>
        /// <param name="userEmailLeaving">Email of the user removed.</param>
        /// <returns>An empty response, unless an error occurs.</returns>
        public string LeaveBoard(int boardId, string userEmailLeaving)
        {
            try
            {
                boardController.leaveBoard(boardId, userEmailLeaving);
                String msg = String.Format("Left Board! userEmailOwner = {0}", userEmailLeaving);
                log.Info(msg);
                Response r = new Response(null);
                return ToJson.toJson(r);
            }
            catch (Exception e)
            {

                //RETURN BAD JASON
                //Response r = new Response(e.Message, false);
                log.Warn(e.Message);
                //return r.BadJson();
                throw new ArgumentException(e.Message);
            }
        }

        /// <summary>
        /// This method assign a user from the board to a task.
        /// </summary>
        /// <param name="userEmailToAssign">Email of the user need to be assign to a task.</param>
        /// <param name="boardName">The name of the new board</param>
        /// <param name="columnOrdinal">The column number.????</param>
        /// <param name="userEmailAssigning">Email of the user assigning other user assign to a task.Must be logged in.</param> 
        /// <param name="taskId">The taskId of the task the userEmailAssigning will assigne </param>
        /// <returns>jason, unless an error occurs .</returns>
        public string AssignTask(string userEmailToAssign, string boardName, int columnOrdinal,
            string userEmailAssigning, int taskId)
        {
            try
            {
                boardController.assignAssignee(userEmailToAssign, boardName, columnOrdinal, userEmailAssigning, taskId);
                String msg = String.Format("task assignee assigned Successfully ! The assignee :{0}",
                    userEmailToAssign);
                log.Info(msg);
                Response r = new Response(null, userEmailToAssign);
                return ToJson.toJson(r);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
            }


        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                //NEED TO USE CHANGEsTATE
                boardController.switchOwnership(currentOwnerEmail, boardName, newOwnerEmail);
                Response r = new Response(null);
                String msg = String.Format("Transfer the Ownership!  new Owner userEmail = {0} of board :{1}",
                    newOwnerEmail, boardName);
                log.Info(msg);

                return ToJson.toJson(r);
            }
            catch (Exception e)
            {

                //RETURN BAD JASON
                Response r = new Response(e.Message);
                log.Warn(e.Message);
                return ToJson.toJson(r);
                //throw new ArgumentException(e.Message);
            }
        }

        /// <summary>
        /// This method delete a board.
        /// </summary>
        /// <param name="name">The name of the board</param>
        /// <param name="userEmail">Email of the user. To identify which board needs to be deleted from which user. </param>
        /// <returns>Response with a command to delete board, unless doesn't exists a board with the same name.</returns>
        public string DeleteBoard(string boardName, string userEmail)
        {
            try
            {
                boardController.DeleteBoard(userEmail, boardName);
                Response r = new Response(null);
                String msg = String.Format("BoardService deleted! userEmail = {0} deleted board :{1}", userEmail,
                    boardName);
                log.Info(msg);

                return ToJson.toJson(r);
            }
            catch (Exception e)
            {

                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
            }
        }

        /// <summary>
        /// This method returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>Response with  a list of the in progress tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string InProgress(string email)
        {
            try
            {
                List<Buissnes_Layer.Task> proCol = boardController.GetAllInPrograss(email);
                Response r = new Response(proCol);
                String msg = String.Format("got InProgress list! userEmail = {0} ", email);
                log.Info(msg);
                return ToJson.toJson(r);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
                //Response r = new Response(e.Message, false);
                //return r.BadJson();
            }

        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with  a list of the column's tasks, unless an error occurs </returns>
        public string GetColum(string email, string boardName, int columnOrdinal)
        {
            try
            {
                List<Buissnes_Layer.Task> allCol = boardController.GetColum(email, boardName, columnOrdinal);
                Response r = new Response(allCol);
                String msg = String.Format("Got the Column! columnOrdinal = {0} ", columnOrdinal);
                log.Info(msg);
                return ToJson.toJson(r);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
            }

        }

        public List<string> GetColumn(string email, string boardName, int columnOrdinal)
        {
            
            List<string> allCol = boardController.GetColum(email, boardName, columnOrdinal).ConvertAll(new Converter<Task, string>(
                (Task t) => { return t.ToString(); }));
            // lambda expression to get names
            String msg = String.Format("Got the Column! columnOrdinal = {0} ", columnOrdinal);
            return allCol;
        }


        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>The string "{}", unless an error occurs </returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                boardController.LimitColumn(email, boardName, columnOrdinal, limit);
                Response r = new Response(null);
                String msg = String.Format("Limit Column has been set! limit = {0}  at columnOrdinal :{1}", limit,
                    columnOrdinal);
                log.Info(msg);
                return ToJson.toJson(r);

            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                //Response r = new Response(e.Message, false);
                //return r.BadJson();
                throw new ArgumentException(e.Message);
            }
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with column name value, unless an error occurs </returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                string colVal = boardController.GetColumnName(email, boardName, columnOrdinal);
                Response r = new Response(null, colVal);
                String msg = String.Format("Got the Column Name! columnOrdinal{0}", columnOrdinal);
                log.Info(msg);
                return colVal;
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with column limit value, unless an error occurs </returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                int colVal = boardController.GetColumnLim(email, boardName, columnOrdinal);
                Response r = new Response(null, colVal);
                String msg = String.Format("Got  the Column Limit! columnOrdinal = {0} ", columnOrdinal);
                log.Info(msg);
                return ToJson.toJson(r);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                //// Console.WriteLine(e);
                //Response r = new Response(e.Message, false);
                //// return JsonSerializer.Serialize(true);
                //return r.BadJson();
                throw new ArgumentException(e.Message);
            }

        }

        public void LoadData()
        {
            this.boardController.LoadData();
        }

        public void DeleteAllData()
        {
            this.boardController.DeleteAllData();
        }



        public Dictionary<int, string> GetUserBoards(string email)
        {
            try
            {
                List<int> boardIDs = boardController.GetUserBList(email);
                Dictionary<int, string> boardNames = new Dictionary<int, string>();
                foreach (int i in boardIDs)
                {
                    boardNames[i] = boardController.GetBoardById(i).name;
                }

                return boardNames;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message); // I hate this!
            }
        }
    }
}
