using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer.Mappers;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.Buissnes_Layer
{
    [Serializable, DataContract]
    public class Task
    {
        [DataMember]
        public int Id { get; }
        [DataMember(Order = 1 )]
        public readonly DateTime CreationTime;
        [DataMember(Order = 1)]
        private string Title { set; get; }
        [DataMember(Order = 1)]
        private string Description { set; get; }
        [DataMember(Order = 1)]
        private DateTime DueDate { set; get; }
        [JsonIgnore]
        private int State;
        [JsonIgnore]
        private static int _ID = 10;
        public int BoardId;
        private TaskDTOMapper taskDTOMapper;

        public string Assignee { private set; get; }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Exception ex = new ArgumentException();
        private const int BacklogState = 0;
        private const int InProgressState = 1;
        private const int Done = 2;
        public Task (string title, DateTime dueDate, int boardId, string description = "", string assignee = "Unassinged")
        {
            this.Id = _ID;
            this.CreationTime = DateTime.Today;
            this.Title = title;
            this.Description = description;
            this.DueDate = dueDate;
            this.State = 0;
            this.Assignee = assignee;
            _ID += 1;
            if (dueDate <= CreationTime)
            {
                throw ex;
            }
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
            BoardId = boardId;
            this.taskDTOMapper = new TaskDTOMapper();
            // Do NOT Load Data!
        }

        /// <summary>
        /// Constructor from TaskDTO - to be used when loading data
        /// </summary>
        /// <param name="taskDto"></param>
        public Task(TaskDTO taskDto)
        {
            this.Title = taskDto.title;
            this.Description = taskDto.description;
            this.Assignee = taskDto.assignee;
            this.DueDate = DateTime.Today;
            this.Id = taskDto.taskID;
            this.BoardId = taskDto.boardID;
            this.CreationTime = DateTime.Today;
            this.State = taskDto.GetState();
            this.taskDTOMapper = new TaskDTOMapper(); // bad?
        }
        public Task(string title, DateTime dueDate, int boardId, string description = "", int ID = 100, string assignee = "Unassinged")
        {
            this.Id = ID;
            this.CreationTime = DateTime.Today;
            this.Title = title;
            this.Description = description;
            this.DueDate = dueDate;
            this.State = 0;
            this.Assignee = assignee;
            if (dueDate <= CreationTime)
            {
                throw ex;
            }
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
            BoardId = boardId;
            this.taskDTOMapper = new TaskDTOMapper();
            // Do NOT Load Data!

        }


        /// <summary>
        /// This method edit the title of a task
        /// </summary>
        ///  <param name="newTitle">The new Title of the task</param>
        /// <returns> nothing, just change it in the tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>

        internal void EditTitle(string newTitle, string email)
        {


            if (String.IsNullOrEmpty(newTitle) || this.State == Done || newTitle.Length>50 ||  IsOnlySpaces(newTitle) || IsHebrew(newTitle) || email!=this.Assignee)

            {
                log.Warn(ex.Message);
                throw ex;
            }
            else
            {
                String msg = String.Format("Task title edited in buisness layer! new title = {0}", newTitle);
                log.Info(msg);
                this.Title = newTitle;
                this.taskDTOMapper.EditTitle(this.Id, newTitle);
            }
            

        }

        internal int GetState()
        {
            return this.State;
        }
        internal string GetStatus()
        {
            {
                if (this.State == BacklogState)
                {
                    return "backlog";
                }
                else if (this.State == InProgressState)
                {
                    return "in progress";
                }
                else if (this.State == Done)
                {
                    return "Done";
                }
                else
                {
                    log.Warn("this column state does not exist!");
                    throw new ArgumentException("this column state does not exist!");
                }
                String msg = String.Format("Got the column name Successfully in BuissnesLayer!");
                log.Info(msg);
            }
        }
        internal void SetState(int state)
        {
             this.State=state;
        }
        /// <summary>
        /// This method edit the description of a task
        /// </summary>
        ///  <param name="newDescription">The description of the task</param>
        /// <returns> nothing, just change it in the tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>

        internal void EditDescription(string newDescription, string email)
        {
            if (newDescription==null || this.State==Done || newDescription.Length > 300 || IsOnlySpaces(newDescription) || IsHebrew(newDescription) || email!=this.Assignee)
            {
                log.Warn(ex.Message);
                throw ex;
            }
            else
            {
                String msg = String.Format("Task description edited in buisness layer! new description = {0}", newDescription);
                log.Info(msg);
                this.Description = newDescription;
                this.taskDTOMapper.EditDescription(this.Id, newDescription);
            }
        }
        /// <summary>
        /// This method edit the due date of a task
        /// </summary>
        ///  <param name="newDueDate">The new due date of the task</param>
        /// <returns> nothing, just change it in the tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>

        internal void EditDueDate(DateTime newDueDate, string email)
        {
            if (newDueDate<=this.CreationTime || email!=this.Assignee || this.State == Done)
            {
                log.Warn(ex.Message);
                throw ex;
            }
            else
            {
                String msg = String.Format("Task due date edited in buisness layer! new due date = {0}", newDueDate);
                log.Info(msg);
                this.DueDate = newDueDate;
                this.taskDTOMapper.EditDueDate(this.Id, newDueDate.ToString());
            }
        }
        /// <summary>
        /// This method edit the assignee of a task
        /// </summary>
        ///  <param name="userEmail">The new assignee of the task</param>
        /// <returns> nothing, just change it in the tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>

        internal void EditAssignee(string userEmail)
        {
            this.Assignee = userEmail;
            this.taskDTOMapper.EditAssignee(this.Id, userEmail);
            
        }
        public string GetTitle()
        {
            return this.Title;
        }
        public string GetDescription()
        {
            return this.Description;
        }
        public string GetDueDate()
        {
            return this.DueDate.ToString();
        }
        private Boolean IsOnlySpaces(string str)
        {
            Regex reg = new Regex(@"^\s*$");
            Match strMatch = reg.Match(str);
            return strMatch.Success;
        }
        private bool IsHebrew(string str)
        {
            string[] heb =
            {
                "א", "ב", "ג", "ד", "ה", "ו", "ז", "ח", "ט", "י", "כ", "ל", "מ", "נ", "ס", "ע", "פ", "צ", "ק", "ר", "ש",
                "ת", "ף", "ץ", "ך", "ן"
            };
            List<string> hebrew = new List<string>(heb);
            for (int i = 0; i < heb.Length; i++)
            {
                if (str.Contains(heb[i]))
                {
                    return true;
                }

            }

            return false;

        }

        // generate ToString
        public override string ToString()
        {
            return "Title: " + this.Title + " Description: " + this.Description + " Due Date: " + this.DueDate + " Assignee: " + this.Assignee + " ID: " + this.Id;
        }
        public int GetId()
        {
            return this.Id;
        }
    }
}
