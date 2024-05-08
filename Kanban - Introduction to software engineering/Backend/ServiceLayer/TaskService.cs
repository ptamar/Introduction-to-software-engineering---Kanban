
using System;
using System.Text.Json;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using log4net.Util;
using Newtonsoft.Json;
using IntroSE.Kanban.Backend.Buissnes_Layer;
using Task = IntroSE.Kanban.Backend.Buissnes_Layer.Task;
using AutoMapper.Internal.Mappers;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.ServiceLayer

{

    public class TaskService
    {
        private BoardController boardController;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public TaskService(BoardController BC)
        {
            this.boardController = BC;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");

        }

        ///// <summary>
        ///// This method creates a new task.
        ///// </summary>
        ///// <param name="email">Email of the user. The user must be logged in.</param>
        ///// <param name="boardName">The name of the board</param>
        ///// <param name="title">Title of the new task</param>
        ///// <param name="description">Description of the new task</param>
        ///// <param name="dueDate">The due date if the new task</param>
        ///// <returns>Response with user-email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        //public string CreateTask(string email, string boardName, string title,string description, string dueDate)
        //{
        //    if (Connections.IsLoggedIn(email))
        //    {
        //        if(BoardService.)
        //    }

        //}
        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="newTitle">New title for the task</param>

        /// <returns>The string "{\"Title\" : \"newTitle\", \"Description\" : \"description\", \"DueDate\" : \"21.04.22\"}", unless an error occurs </returns>
        public string EditTitle(string email, string boardName, int taskId, string newTitle)
        {
            try
            {
                Task task = boardController.GetTask(email, boardName, taskId);
                try
                {
                    task.EditTitle(newTitle, email);
                    Response response = new Response(null, task);
                    String msg = String.Format("Task title edited! new title = {0}", newTitle);
                    log.Info(msg);
                    Console.WriteLine(msg);
                    Console.WriteLine(response.OKJson());
                    return response.OKJson();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    log.Warn(ex.Message);
                    Response response = new Response(ex.Message);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                log.Warn(ex.Message);
                Response response = new Response(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="newDescription">New description for the task</param>
        /// <returns>The string "{\"Title\" : \"title\", \"Description\" : \"newDescription\", \"DueDate\" : \"21.04.22\"}", unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string EditDescription(string email, string boardName, int taskId, string newDescription)
        {
            try
            {
                Task task = boardController.GetTask(email, boardName,taskId);
                try
                {
                    task.EditDescription(newDescription, email);
                    Response response = new Response(null, task);
                    String msg = String.Format("Task description edited! new description = {0}", newDescription);
                    log.Info(msg);
                    Console.WriteLine(msg);
                    return response.OKJson();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    log.Warn(ex.Message);
                    Response response = new Response(ex.Message);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                log.Warn(ex.Message);
                Response response = new Response(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="newDueDate">The new due date of the column</param>
        /// <returns>The string "{\"Title\" : \"title\", \"Description\" : \"description\", \"DueDate\" : \"newDueDate"}", unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string EditDueDate(string email, string boardName, int taskId, DateTime newDueDate)
        {
            try
            {
                var task = boardController.GetTask(email, boardName, taskId);
                task.EditDueDate(newDueDate,email);
                var response = new Response(null);
                var msg = $"Task due date edited! new due date = {newDueDate}";
                log.Info(msg);
                return ToJson.toJson(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                log.Warn(ex.Message);
                var response = new Response(ex.Message);
                throw ex;
            }
        }
            
        }
    }
