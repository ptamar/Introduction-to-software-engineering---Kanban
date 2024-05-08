using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Mappers
{
    internal class BoardDTOMapper
    {
        private BoardUsersMapper boardUsersMapper;
        private List<BoardDTO> boardDTOs = new List<BoardDTO>();
        private TaskDTOMapper taskDTOMapper;
        private int boardCount;
        public int BoardCount => boardCount;
        const string tableName = "Boards";
        const string BoardUsersTable = "Board_Users";
        private const string TasksTable = "Tasks";
        private const string idColumn = "ID";
        private const string nameColumn = "Name";
        private const string ownerColumn = "Owner_email";
        private const string backlogMaxColumn = "Backlog_max";
        private const string inProgressMaxColumn = "In_Progress_max";
        private const string doneMaxColumn = "Done_max";
        private const int backlogMax = -1; //Default values
        private const int inProgressMax = -1; //Default values
        private const int doneMax = -1; //Default values
        private Dictionary<int, string> columnNamesByOrdinal;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal BoardDTOMapper()
        {
            this.boardUsersMapper = new BoardUsersMapper();
            this.boardCount = 0;// LoadData and update count
            this.taskDTOMapper = new TaskDTOMapper();
            columnNamesByOrdinal = new Dictionary<int, string>();
            columnNamesByOrdinal[0] = backlogMaxColumn;
            columnNamesByOrdinal[1] = inProgressMaxColumn;
            columnNamesByOrdinal[2] = doneMaxColumn;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
        }

        internal void AddUserToBoard(int boardID, string email)
        {
            this.boardUsersMapper.AddUserToBoard(boardID, email);

        }

        /// <summary>
        /// Removes a user from membership in a board
        /// </summary>
        /// <param name="boardID"></param>
        /// <param name="email"></param>
        internal void RemoveUserFromBoard(int boardID, string email)
        {
            this.boardUsersMapper.RemoveUser(boardID, email);
            String msg = String.Format("RemoveUserFromBoard Successfully in BoardDTOM!!");
            log.Info(msg);
        }

        /// <summary>
        /// Creates a board in the DB.
        /// </summary>
        /// <param name="ownerEmail"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        /// <exception cref="DALException"></exception>
        internal BoardDTO CreateBoard(string ownerEmail, string boardName)
        {
            string path = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), "kanban.db"));
            SQLiteConnectionStringBuilder builder = new() { DataSource = path };
            // string connectionString = $"Data Source={path}; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {tableName} ({idColumn}, {nameColumn}, {ownerColumn}," +
                                          $"{backlogMaxColumn}, {inProgressMaxColumn}, {doneMaxColumn}) " +
                                          $"VALUES (@id_val, @name_val,@email_val," +
                                          $" @backlog_val,@inProgress_val, @done_val);";

                    SQLiteParameter ownerParam = new SQLiteParameter(@"email_val", ownerEmail);
                    SQLiteParameter nameParam = new SQLiteParameter(@"name_val", boardName);
                    SQLiteParameter idParam = new SQLiteParameter(@"id_val", boardCount);
                    SQLiteParameter backlogParam = new SQLiteParameter(@"backlog_val", backlogMax);
                    SQLiteParameter inProgressParam = new SQLiteParameter(@"inProgress_val", inProgressMax);
                    SQLiteParameter doneParam = new SQLiteParameter(@"done_val", doneMax);


                    command.Parameters.Add(ownerParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(backlogParam);
                    command.Parameters.Add(inProgressParam);
                    command.Parameters.Add(doneParam);

                    command.Prepare();
                    res = command.ExecuteNonQuery();


                    BoardDTO board = new BoardDTO(owner: ownerEmail,
                        name: boardName, iD: boardCount, backlogMax: backlogMax,
                        inProgressMax: inProgressMax, doneMax: doneMax);
                    boardDTOs.Add(board);
                    boardUsersMapper.CreateBoard(boardCount, ownerEmail); //bug? Are we not trying to access the DB while it's open?
                    boardCount++;
                    String msg = String.Format("CreateBoard Successfully in BoardDTOM!!");
                    log.Info(msg);
                    return board;
                }
                catch (SQLiteException ex)
                {
                    //Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.Message);
                    log.Warn(ex.Message);
                    throw new DALException($"Create user failed because " + ex.Message);
                    // log error
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

                return null; // If failed to create user
            }
        }

        /// <summary>
        /// Do NOT use! Use DeleteBoard(string ownerEmail, string boardName, int boardID)
        /// </summary>
        /// <param name="board_Id"></param>
        /// <returns></returns>
        internal bool DeleteBoard(int board_Id)
        {
            throw new NotImplementedException("Do NOT use! Use DeleteBoard(string ownerEmail, string boardName, int boardID");
        }

        /// <summary>
        /// Deletes a board from the DB
        /// </summary>
        /// <param name="ownerEmail">Email of the board owner</param>
        /// <param name="boardName">Name of the board</param>
        /// <param name="boardID">ID of the board</param>
        internal void DeleteBoard(string ownerEmail, string boardName, int boardID)
        {

            {
                string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
                SQLiteConnectionStringBuilder builder = new() { DataSource = path };
                string connectionString = $"Data Source={path}; Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(builder.ConnectionString))
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    int res = -1;
                    try
                    {
                        connection.Open();
                        command.CommandText = $"DELETE FROM {tableName} " +
                                              $"WHERE {nameColumn} = @board_name AND " +
                                              $"{ownerColumn} = @username";

                        SQLiteParameter boardParam = new SQLiteParameter(@"board_name", boardName);
                        SQLiteParameter userParam = new SQLiteParameter(@"username", ownerEmail);
                        command.Parameters.Add(boardParam);
                        command.Parameters.Add(userParam);

                        command.Prepare();
                        res = command.ExecuteNonQuery();
                        boardDTOs.RemoveAll(x => x.Owner == ownerEmail && x.Name == boardName && x.ID == boardID);
                        boardUsersMapper.DeleteBoard(boardID);
                        String msg = String.Format("DeleteBoard Successfully in BoardDTOM!!");
                        log.Info(msg);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(command.CommandText);
                        log.Warn(ex.Message);
                        Console.WriteLine(ex.Message);
                        // log error
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                    }
                }
            }
        }
        /// <summary>
        /// Changes the board ownership in the database. Does not change the BoardDTO.
        /// </summary>
        /// <param name="newOwner"></param>
        /// <param name="boardID"></param>
        /// <exception cref="DALException"></exception>
        public void ChangeOwnership(string newOwner, int boardID)
        {
            string path = Path.GetFullPath(Path.Combine(
               Directory.GetCurrentDirectory(), "kanban.db"));
            SQLiteConnectionStringBuilder builder = new() { DataSource = path };
            string connectionString = $"Data Source={path}; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);



                int res = -1;

                try
                {
                    connection.Open();
                    command.Prepare();
                    // Console.WriteLine(res);
                    // Console.WriteLine("success!");
                    command.CommandText = $"UPDATE {tableName} SET {ownerColumn} = @ownerID_val WHERE {idColumn} = @boardID_val";
                    SQLiteParameter ownerParam = new SQLiteParameter(@"ownerID_val", newOwner);
                    SQLiteParameter boardIDParam = new SQLiteParameter(@"boardID_val", boardID);
                    command.Parameters.Add(ownerParam);
                    command.Parameters.Add(boardIDParam);
                    res = command.ExecuteNonQuery();
                    String msg = String.Format("ChangeOwnership Successfully in BoardDTOM!!");
                    log.Info(msg);


                }
                catch (SQLiteException ex)
                {
                    //Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.Message);
                    log.Warn(ex.Message);
                    throw new DALException($"Change owner failed because " + ex.Message);
                    // log error
                }
                finally
                {

                    command.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// Updates the column limit in the DB.
        /// </summary>
        /// <param name="boardId">BoardID</param>
        /// <param name="columnToChange">Column ordinal of the Column to change:
        /// {Backlog: 0, InProgress: 1, Done: 2}</param>
        /// <param name="newLimit">New limit</param>
        /// <exception cref="DALException"></exception>
        internal void ChangeColumnLimit(int boardId, int columnToChange, int newLimit)
        {

            string path = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), "kanban.db"));
            SQLiteConnectionStringBuilder builder = new() { DataSource = path };
            string connectionString = $"Data Source={path}; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();

                    command.Prepare();

                    // Console.WriteLine(res);
                    // Console.WriteLine("success!");
                    string columnName = columnNamesByOrdinal[columnToChange];
                    command.CommandText = $"UPDATE {tableName} SET {columnName} = @limit_val WHERE {idColumn} = @boardID_val";
                    //SQLiteParameter columnParam = new SQLiteParameter(@"column_val", columnNamesByOrdinal[columnToChange]);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limit_val", newLimit);
                    SQLiteParameter boardIDParam = new SQLiteParameter(@"boardID_val", boardId);
                    //command.Parameters.Add(columnParam);
                    command.Parameters.Add(limitParam);
                    command.Parameters.Add(boardIDParam);
                    res = command.ExecuteNonQuery();
                    String msg = String.Format("ChangeColumnLimit Successfully in BoardDTOM!!");
                    log.Info(msg);

                }
                catch (SQLiteException ex)
                {
                    //Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.Message);
                    log.Warn(ex.Message);
                    throw new DALException($"Change column limit failed because " + ex.Message);
                    // log error
                }
                finally
                {

                    command.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Loads all boards and creates BoardDTO objects
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DALException"></exception>
        public List<BoardDTO> LoadBoards()
        {
            string path = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), "kanban.db"));

            SQLiteConnectionStringBuilder builder = new() { DataSource = path };
            string connectionString = $"Data Source={path}; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"Select * FROM {tableName}; " +
                                          $"SELECT max({idColumn}) FROM {tableName};";
                    command.Prepare();
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int ID = (int)Convert.ToInt64(reader["ID"]);
                        string owner = reader["Owner_email"].ToString();
                        string name = reader["Name"].ToString();
                        int BacklogMax = (int)Convert.ToInt64(reader["Backlog_max"]);
                        int InProgressMax = (int)Convert.ToInt64(reader["In_Progress_max"]);
                        int DoneMax = (int)Convert.ToInt64(reader["Done_max"]);
                        BoardDTO board = new BoardDTO(owner: owner,
                            name: name, iD: ID, backlogMax: BacklogMax,
                            inProgressMax: InProgressMax, doneMax: DoneMax);
                        boardDTOs.Add(board);
                        Console.WriteLine("Board " + ID + " loaded successfully");
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        int nextBoardID = (int)Convert.ToInt64(reader["max(ID)"]);
                        this.boardCount = nextBoardID;
                    }
                    String msg = String.Format("LoadBoards Successfully in BoardDTOM!!");
                    log.Info(msg);
                    return boardDTOs;

                }
                catch (System.InvalidCastException e)
                {
                    this.boardCount = 0;
                    command.Dispose();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    this.boardCount = 0;
                    command.Dispose();
                    connection.Close();
                    log.Warn(ex.Message);
                    throw new DALException($"Load data failed because " + ex.Message);
                    // log error
                    // Maybe throw an exception? Probs not, might not reach finally
                }
                finally
                {
                    // Console.WriteLine("Reached Finally");
                    command.Dispose();
                    connection.Close();

                }
            }

            List<BoardDTO> ifFailed = new List<BoardDTO>();
            return ifFailed;
        }

        /// <summary>
        /// Deletes all database data from Boards, Board_Users and Tasks
        /// </summary>
        public void DeleteAllData()
        {
            string path = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), "kanban.db"));
            SQLiteConnectionStringBuilder builder = new() { DataSource = path };
            //string connectionString = $"Data Source={path}; Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {tableName};" +
                                          $"DELETE FROM {BoardUsersTable};" +
                                          $"DELETE FROM {TasksTable};";
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    boardUsersMapper.DeleteAllData(); // Deletes all Board_Users table
                    foreach (var boardDTO in boardDTOs)
                    {
                        // Major bug - opening a new SQL connection while the DB is still open.
                        // causes DB to be locked!
                        boardDTO.DeleteAllData();// Deletes all tasks
                    }
                    boardDTOs.Clear();
                    Console.WriteLine($"SQL execution finished without errors. Result: {res} rows changed(deleted)");
                    String msg = String.Format("DeleteAllData Successfully in BoardDTOM!!");
                    log.Info(msg);
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.Message);
                    log.Warn(ex.Message);
                    throw new DALException($"Delete data failed because " + ex.Message);
                    // log error
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
        }
        ///<summary>
        ///This function should make a query and return the number of tasks so we don't use the same ID twice
        ///</summary>
        ///<returns>The number of tasks</returns>
        public int getNumberOfTasks()
        {
            int count;
            string path = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), "kanban.db"));
            SQLiteConnectionStringBuilder builder = new() { DataSource = path };
            using (SQLiteConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"SELECT COUNT(*) FROM {TasksTable};";
                    command.Prepare();
                    count = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Number of tasks: {count}");
                    String msg = String.Format("getNumberOfTasks Successfully in BoardDTOM!!");
                    log.Info(msg);
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.Message);
                    log.Warn(ex.Message);
                    throw new DALException($"Error retrieving the number of tasks: " + ex.Message);
                    // log error
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return count;
        }
    }
    } 


