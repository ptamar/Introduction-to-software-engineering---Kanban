using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions.Impl;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Mappers
{
    internal class BoardUsersMapper
    {
        private List<BoardUsersDTO> _boardUsersDTOs;
        const string boardColumnName = "Board_Id";
        const string userColumnName = "User_email";
        const string tableName = "Board_Users";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal BoardUsersMapper()
        {
            this._boardUsersDTOs = new List<BoardUsersDTO>();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
        }

        /// <summary>
        /// Creates a board and adds the owner as a board user
        /// </summary>
        /// <param name="boardID"></param>
        /// <param name="userEmail"></param>
        public void CreateBoard(int boardID, string userEmail)
        {
            // Currently passes tests
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
                    command.CommandText = $"INSERT INTO {tableName} ({boardColumnName}, {userColumnName}) " +
                                          $"VALUES (@board_id, @username)";

                    SQLiteParameter boardParam = new SQLiteParameter(@"board_id", boardID);
                    SQLiteParameter userParam = new SQLiteParameter(@"username", userEmail);

                    command.Parameters.Add(boardParam);
                    command.Parameters.Add(userParam);

                    command.Prepare();
                    res = command.ExecuteNonQuery();


                    BoardUsersDTO boardUser = new BoardUsersDTO(boardID, userEmail);
                    _boardUsersDTOs.Add(boardUser);
                    String msg = String.Format("CreateBoard Successfully in BoardUserDTOM!!");
                    log.Info(msg);

                    // Console.WriteLine(res);
                    // Console.WriteLine("success!");
                    //return boardUser; // In case we want to return to return a boardUser object.


                }
                catch (Exception ex)
                {
                    Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.Message);
                    log.Warn(ex.Message);
                    // log error
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
        }


        public void AddUserToBoard(int boardID, string userEmail)
        {
            // Currently passes tests
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
                    command.CommandText = $"INSERT INTO {tableName} ({boardColumnName}, {userColumnName}) " +
                                          $"VALUES (@board_id, @username)";

                    SQLiteParameter boardParam = new SQLiteParameter(@"board_id", boardID);
                    SQLiteParameter userParam = new SQLiteParameter(@"username", userEmail);

                    command.Parameters.Add(boardParam);
                    command.Parameters.Add(userParam);

                    command.Prepare();
                    res = command.ExecuteNonQuery();


                    BoardUsersDTO boardUser = new BoardUsersDTO(boardID, userEmail);
                    _boardUsersDTOs.Add(boardUser);
                    String msg = String.Format("User added to BoardUsers table in DB");
                    log.Info(msg);
                    // Console.WriteLine(res);
                    // Console.WriteLine("success!");
                    //return boardUser; // In case we want to return to return a boardUser object.


                }
                catch (Exception ex)
                {
                    Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.Message);
                    log.Warn(ex.Message);
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
        /// Deletes all instances of the boardID from the database.
        /// </summary>
        /// <param name="boardID">ID of the board to delete</param>
        public void DeleteBoard(int boardID)
        {
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
                    command.CommandText = $"DELETE FROM {tableName} " +
                                          $"WHERE {boardColumnName} = @board_id";

                    SQLiteParameter boardParam = new SQLiteParameter(@"board_id", boardID);
                    command.Parameters.Add(boardParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    _boardUsersDTOs.RemoveAll(x => x.BoardID == boardID);
                    String msg = String.Format("DeleteBoard Successfully in BoardUserDTOM!!");
                    log.Info(msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.Message);
                    log.Warn(ex.Message);
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
        /// Terminates the membership of a user in a board
        /// </summary>
        /// <param name="boardID"></param>
        /// <param name="userEmail"></param>
        public void RemoveUser(int boardID, string userEmail)
        {
            {
                string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
                string connectionString = $"Data Source={path}; Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    int res = -1;
                    try
                    {
                        connection.Open();
                        command.CommandText = $"DELETE FROM {tableName} " +
                                              $"WHERE {boardColumnName} = @board_id AND" +
                                              $"{userColumnName} = @username";

                        SQLiteParameter boardParam = new SQLiteParameter(@"board_id", boardID);
                        SQLiteParameter userParam = new SQLiteParameter(@"username", userEmail);
                        command.Parameters.Add(boardParam);
                        command.Parameters.Add(userParam);

                        command.Prepare();
                        res = command.ExecuteNonQuery();
                        _boardUsersDTOs.RemoveAll(x => x.BoardID == boardID && x.userName == userEmail);
                        String msg = String.Format("RemoveUser Successfully in BoardUserDTOM!!");
                        log.Info(msg);

                        //BoardUsersDTO boardUser = new BoardUsersDTO(boardID, userEmail);
                        //_boardUsersDTOs.Add(boardUser);

                        // Console.WriteLine(res);
                        // Console.WriteLine("success!");
                        //return boardUser; // In case we want to return to return a boardUser object.
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(command.CommandText);
                        Console.WriteLine(ex.Message);
                        log.Warn(ex.Message);
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

        public void DeleteAllData()
        {
            this._boardUsersDTOs.Clear();
            String msg = String.Format("DeleteAllData Successfully in BoardUserDTOM!!");
            log.Info(msg);
        }
    }
}
      

