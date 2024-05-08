using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.Mappers;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class BoardDTO
    {
        private string owner;
        public string Owner => owner;
        private string name;

        public string Name => name;
        private readonly int iD;
        public int ID => iD;
        private int backlogMax;

        public int BacklogMax => backlogMax;
        private int inProgressMax;
        public int InProgressMax => inProgressMax;
        private int doneMax;
        public int DoneMax => doneMax;
        private string TasksTable = "Tasks";
        private string BoardUsersTable = "Board_Users";
        public List<TaskDTO> taskDTOs = new List<TaskDTO>();
        private List<string> BoardUsers = new List<string>();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private TaskDTOMapper taskDTOMapper;

        /// <summary>
        /// Do Not Use!! This is an old constructor - consult Peleg before using
        /// </summary>
        public BoardDTO() 
        {
            throw new NotImplementedException("Do not USE!");
        }



        public BoardDTO(string owner, string name, int iD, int backlogMax, int inProgressMax, int doneMax)
        {
            //, List<string> boardUsers)
            this.taskDTOMapper = new TaskDTOMapper();
            this.owner = owner;
            this.name = name;
            this.iD = iD;
            this.backlogMax = backlogMax;
            this.inProgressMax = inProgressMax;
            this.doneMax = doneMax;
            //BoardUsers = boardUsers;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");

        }

        /// <summary>
        /// Loads the users and tasks of a board and creates a board DTO.
        /// </summary>
        /// <returns></returns>
        public BoardDTO LoadBoard()
        {

            string path = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), "kanban.db"));
            string connectionString = $"Data Source={path}; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"Select * FROM {TasksTable} " +
                                          $"WHERE Board_ID = {this.iD}; " +
                                          $"SELECT * FROM {BoardUsersTable} " +
                                          $"WHERE Board_ID = {this.iD};";
                    command.Prepare();
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    { // Create Task DTOs and add Tasks
                        int taskID = (int)Convert.ToInt64(reader["Task_ID"]);
                        int boardID = (int)Convert.ToInt64(reader["Board_ID"]);
                        string assignee = reader["Assignee"].ToString();
                        string status = reader["Status"].ToString();
                        string title = reader["Title"].ToString();
                        string description = reader["Description"].ToString();
                        string dueDate = reader["Due_Date"].ToString();
                        string creationTime = reader["Creation_Time"].ToString();
                        TaskDTO task = new TaskDTO(taskID: taskID, boardID: boardID,
                            assignee: assignee, status: status,
                            title: title, description: description,
                            dueDate: dueDate, creationTime: creationTime);
                        taskDTOs.Add(task);
                        Console.WriteLine("Task " + taskID + " loaded to Board " + boardID);
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        // Add Users
                        string email = reader["User_email"].ToString();
                        BoardUsers.Add(email);
                        
                    }
                    String msg = String.Format("loaded Data Successfully in BoardDTO!!");
                    log.Info(msg);
                    return this;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.Message);
                    log.Warn(ex.Message);
                    // log error
                    // Maybe throw an exception? Probs not, might not reach finally
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

                BoardDTO ifFailed = null;
                return ifFailed;
            }
        }

        /// <summary>
        /// Deletes the DTO's lists. Does not access the database.
        /// </summary>
        public void DeleteAllData()
        {
            try
            {
                this.BoardUsers.Clear();
                this.taskDTOs.Clear();
                String msg = String.Format("deleted Data Successfully in BoardDTO!!");
                log.Info(msg);
            }

            catch (NullReferenceException e) {}
            
        }
        /*
        string path = Path.GetFullPath(Path.Combine(
            Directory.GetCurrentDirectory(), "kanban.db"));
        string connectionString = $"Data Source={path}; Version=3;";

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);



            int res = -1;

            try
            {
                connection.Open();
                command.CommandText = $"DELETE FROM {TasksTable}";
                command.Prepare();
                res = command.ExecuteNonQuery();
                taskDTOs.Clear();
                Console.WriteLine($"SQL execution finished without errors. Result: {res} rows changed");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(command.CommandText);
                Console.WriteLine(ex.Message);
                throw new DALException($"Delete tasks failed because " + ex.Message);
                // log error
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }
        }
        */

        
        public TaskDTO AddTask(int taskID, int boardID, string assignee, string status, string title, string description, string dueDate, string creationTime)
        {
            TaskDTO newTask = taskDTOMapper.CreateTask(taskID, boardID, assignee, status, title, description, dueDate, creationTime);
            String msg = String.Format("added task Successfully ib BoardDTO!!");
            log.Info(msg);
            return newTask;
        
        }


        
    }
}
