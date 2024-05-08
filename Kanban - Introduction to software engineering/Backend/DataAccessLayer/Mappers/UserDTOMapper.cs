using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Mappers
{
    internal class UserDTOMapper
    {
        private List<UserDTO> userDTOs = new List<UserDTO>();
        const string emailColumnName = "email";
        const string passwordColumnName = "password";
        const string tableName = "Users";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserDTOMapper()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
        }
        /// <summary>
        /// Creates a user in the database and instantiates a user DTO.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="DALException"></exception>
        public UserDTO CreateUser(string email, string password)
        {

            string path = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), "kanban.db"));
            SQLiteConnectionStringBuilder builder = new() { DataSource = path };

            using (SQLiteConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {tableName}" +
                                          $" ({emailColumnName}, {passwordColumnName}) " +
                                        $"VALUES (@email_val, @password_val)";
                    SQLiteParameter emailParam = new SQLiteParameter(@"email_val", email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"password_val", password);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    // Console.WriteLine(res);
                    // Console.WriteLine("success!");

                    UserDTO user = new UserDTO(email, password);
                    userDTOs.Add(user);
                    String msg = String.Format("EditAssignee Successfully in UserDTOMapper!!");
                    log.Info(msg);
                    return user;


                    //
                    // command.CommandText = "Select * FROM Users";
                    //
                    // SQLiteDataReader reader = command.ExecuteReader();
                    // while (reader.Read())
                    // {
                    //      Console.WriteLine(reader["email"] + ", " + reader["password"]);
                    // }
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
            }
            return null; // If failed to create user
        }
        /// <summary>
        /// Loads all users and creates UserDTO objects
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DALException"></exception>
        internal List<UserDTO> LoadUsers()
        {
            string path = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), "kanban.db"));
            SQLiteConnectionStringBuilder builder = new() { DataSource = path };

            using (SQLiteConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"Select * FROM {tableName}";
                    command.Prepare();
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string username = reader["email"].ToString();
                        string password = reader["password"].ToString();
                        UserDTO user = new UserDTO(username, password);
                        userDTOs.Add(user);
                        Console.WriteLine("User " + username + " loaded successfully");
                        
                    }

                    String msg = String.Format("LoadUsers Successfully in UserDTOMapper!!");
                    log.Info(msg);
                    return userDTOs;

                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(command.CommandText);
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
            }

            List<UserDTO> ifFailed = new List<UserDTO>();
            return ifFailed;
        }

        /// <summary>
        /// Deletes all database data from Users and Board_Users.
        /// </summary>
        public void DeleteAllData()
        {
            string path = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), "kanban.db"));
            SQLiteConnectionStringBuilder builder = new() { DataSource = path };

            using (SQLiteConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;

                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {tableName};";
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    Console.WriteLine($"SQL execution finished without errors. Result: {res} rows changed");
                    userDTOs.Clear();
                    String msg = String.Format("DeleteAllData Successfully in UserDTOMapper!!");
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
    }
}
