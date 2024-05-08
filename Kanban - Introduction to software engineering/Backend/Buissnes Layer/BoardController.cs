using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer.Mappers;
using log4net;
using log4net.Config;


namespace IntroSE.Kanban.Backend.Buissnes_Layer
{
    public class BoardController
    {

        private Dictionary<string, Dictionary<string,Board>> BoardsOfUsers = new Dictionary<string, Dictionary<string, Board>>();
        private Dictionary<string,List<string>> ownerBoards = new Dictionary<string, List<string>>();
        public UserController userController;
        private const int BacklogState = 0;
        private const int inProgressState = 1;
        private const int Done = 2;
        
        public int BID
        {
            get
            {
                return this.bID;

            }

            set
            {
                this.bID = value;
            }
        }
        
        private int bID;
        private BoardDTOMapper boardDTOMapper;
        private Dictionary<int,Board> boardById = new Dictionary<int,Board>();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BoardController(UserController UC)
        { 
            this.userController = UC;
            this.boardDTOMapper = new BoardDTOMapper();
            // this.boardDTOMapper.LoadData(); Do NOT activate! Ask Peleg why
            // (constructors must not load data - if they throw an exception the entire program fails)
            this.BID = boardDTOMapper.BoardCount;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting log!");
        }
        /// <summary>
        /// This method checks if a user has any board.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <returns>bool </returns>
        public bool UserHasAnyBoard(string userEmail) //checks if user has any board
        {
            if (BoardsOfUsers.ContainsKey(userEmail))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        
        /// <summary>
        /// This method checks if a user has a certain board already.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>bool </returns>
        public bool UserHasThisBoard(string userEmail,string boardName) //checks if board exists
        {
            
            if (this.BoardsOfUsers[userEmail].ContainsKey(boardName))
            { 
                
                return true;
            }
            else 
            { 
                return false;
            }

            
        }

        public bool isOwnerOfAnyBoard(string userEmail)
        {
            if (ownerBoards.ContainsKey(userEmail))
            {
                return true;
            }

            return false;
        }

        public Board GetBoardById( int boardID)
        {
            if (boardById.ContainsKey(boardID))
            {
                String msg = String.Format("GetBoardById Successfully in BuissnesLayer! the Board {0}", boardID);
                log.Info(msg);
                return this.boardById[boardID];
            }
            else
            {
                log.Warn("THIS BOARD DOES NOT EXSIT");
                throw new Exception("THIS BOARD DOES NOT EXSIT");
            }
        }
        /// <summary>
        /// This method Gets User Board ID List to a specific user.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <returns>List of Board ID List, unless an error occurs </returns>
        public List<int> GetUserBList(string userEmail)
        {
            if (userController.IsLoggedIn(userEmail))
            {
                if (UserHasAnyBoard(userEmail))
                {
                    List<int> listOfUserBoard = new List<int>();
                    foreach (var i in this.BoardsOfUsers[userEmail])
                    {
                        listOfUserBoard.Add(i.Value.BoardID);
                    }
                    String msg = String.Format("Got User BList Successfully in BuissnesLayer! Board of {0}", userEmail);
                    log.Info(msg);
                    return listOfUserBoard;
                }
                else
                {
                    List<int> listOfUserBoard = new List<int>();
                    String msg = String.Format("Got User BList Successfully in BuissnesLayer!{0} has None boards", userEmail);
                    log.Info(msg);
                    return listOfUserBoard;

                }
               
            }
            else
            {
                log.Warn("user not logged in");
                throw new ArgumentException("user not logged in");
            }

            
        }
        /// <summary>
        /// This method adds a board to the specific user.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>void, unless an error occurs </returns>
        public void CreateBoard(string userEmail, string boardName)
        {
            try
            {
                if (userController.IsLoggedIn(userEmail))
                {
                    if (UserHasAnyBoard(userEmail))
                    {
                        if (!UserHasThisBoard(userEmail, boardName))
                        {
                            if (isOwnerOfAnyBoard(userEmail))
                            {
                                Board newBoard = new Board(boardDTOMapper.CreateBoard(userEmail, boardName));
                                //new Board(boardName, this.bId, userEmail); - old constructor, do not use
                                newBoard.AddToJoinList(userEmail);// the owner is a joiner as well
                                this.boardById.Add(newBoard.BoardID ,newBoard);
                                this.ownerBoards[userEmail].Add(newBoard.name);
                                BID++;
                                this.BoardsOfUsers[userEmail].Add(boardName, newBoard);
                            }
                            else
                            {
                                Board newBoard = new Board(boardDTOMapper.CreateBoard(userEmail, boardName));
                                List<string> listBoard= new List<string>();
                                listBoard.Add(newBoard.name);
                                newBoard.AddToJoinList(userEmail);// the owner is a joiner as well
                                this.boardById.Add(newBoard.BoardID, newBoard);
                                this.ownerBoards.Add(userEmail, listBoard);
                                BID++;
                                this.BoardsOfUsers[userEmail].Add(boardName, newBoard);
                            }
                        }
                        else
                        {
                            log.Warn("USER CANNOT CREATE THIS BOARD! USER HAS A BOARD WITH THIS NAME ALREADY");
                            throw new ArgumentException("USER CANNOT CREATE THIS BOARD! USER HAS A BOARD WITH THIS NAME ALREADY");
                        }

                    }
                    else
                    {
                        Board newBoard = new Board(boardDTOMapper.CreateBoard(userEmail, boardName));
                        List<string> listBoard = new List<string>();
                        listBoard.Add(newBoard.name);
                        newBoard.AddToJoinList(userEmail);// the owner is a joiner as well
                        this.boardById.Add(newBoard.BoardID, newBoard);
                        this.ownerBoards.Add(userEmail, listBoard);
                        BID++;
                        Dictionary<string, Board> board = new Dictionary<string, Board>();
                        board.Add(boardName, newBoard);
                        BoardsOfUsers.Add(userEmail, board);
                    }
                    String msg = String.Format("Board created Successfully in BuissnesLayer! The Board {0} to email :{1}", boardName, userEmail);
                    log.Info(msg);
                }
                else
                {
                    log.Warn("user not logged in");
                    throw new ArgumentException("user not logged in");
                }
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
            }
            
        }

        internal void LoadBoard(string userEmail, string boardName, BoardDTO boardDto)
        {
            try
            {
                if (UserHasAnyBoard(userEmail))
                {
                    if (!UserHasThisBoard(userEmail, boardName))
                    {
                        if (isOwnerOfAnyBoard(userEmail))
                        {
                            Board newBoard = new Board(boardDto);
                            //new Board(boardName, this.bId, userEmail); - old constructor, do not use
                            newBoard.AddToJoinList(userEmail);// the owner is a joiner as well
                            this.boardById.Add(newBoard.BoardID, newBoard);
                            this.ownerBoards[userEmail].Add(newBoard.name);
                            BID++;
                            this.BoardsOfUsers[userEmail].Add(boardName, newBoard);
                            
                        }
                        else
                        {
                            Board newBoard = new Board(boardDto);
                            List<string> listBoard = new List<string>();
                            listBoard.Add(newBoard.name);
                            newBoard.AddToJoinList(userEmail);// the owner is a joiner as well
                            this.boardById.Add(newBoard.BoardID, newBoard);
                            this.ownerBoards.Add(userEmail, listBoard);
                            BID++;
                            this.BoardsOfUsers[userEmail].Add(boardName, newBoard);
                        }
                    }
                    else
                    {
                        log.Warn("USER CANNOT CREATE THIS BOARD! USER HAS A BOARD WITH THIS NAME ALREADY");
                        throw new ArgumentException("USER CANNOT CREATE THIS BOARD! USER HAS A BOARD WITH THIS NAME ALREADY");
                    }

                }
                else
                {
                    Board newBoard = new Board(boardDto);
                    List<string> listBoard = new List<string>();
                    listBoard.Add(newBoard.name);
                    newBoard.AddToJoinList(userEmail);// the owner is a joiner as well
                    this.boardById.Add(newBoard.BoardID, newBoard);
                    this.ownerBoards.Add(userEmail, listBoard);
                    BID++;
                    Dictionary<string, Board> board = new Dictionary<string, Board>();
                    board.Add(boardName, newBoard);
                    BoardsOfUsers.Add(userEmail, board);
                }
                String msg = String.Format("Board created Successfully in BuissnesLayer! The Board {0} to email :{1}", boardName, userEmail);
                log.Info(msg);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
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
        public void AddTaskB(string email, string boardName, string title, string description, DateTime dueDate)
        {
            if (userController.IsLoggedIn(email))
            {
                try
                {
                    if (UserHasAnyBoard(email))
                    {
                        if (UserHasThisBoard(email, boardName))
                        {
                            Board b = GetBoard(email, boardName);
                            int ID = boardDTOMapper.getNumberOfTasks() + 1;
                            b.AddTask(title, description, dueDate, email, ID);
                            String msg = String.Format("task added Successfully! to board :{0}", boardName);
                            log.Info(msg);
                        }
                        else
                        {
                            log.Warn("USER DON'T HAVE THIS BOARD!");
                            throw new Exception("USER DON'T HAVE THIS BOARD!");
                        }
                    }
                    else
                    {
                        log.Warn("USER DON'T HAVE ANY BOARD!");
                        throw new Exception("USER DON'T HAVE ANY BOARD!");
                    }
                   
                    
                    // try
                    // {
                    
                    // }
                    // catch (Exception e)
                    // {
                    //     log.Warn(e.Message);
                    //     throw new Exception(e.Message);
                    // }

                }
                catch (Exception e)
                {
                    log.Warn(e.Message);
                    throw new Exception(e.Message);
                }

            }
            else
            {
                log.Warn("user not logged in");
                throw new ArgumentException("user not logged in");
            }
           
           
        }
        /// <summary>
        /// This method updates the state of the  task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>VOID.</returns>
        public void NextStateB(string email, string boardName, int columnOrdinal, int taskId)
        {
            if (userController.IsLoggedIn(email))
            {
                try
                {
                    if (UserHasThisBoard(email,boardName) && taskId != null)
                    {
                        if (GetBoard(email, boardName).GetTask(taskId).GetState() == columnOrdinal)
                        {
                            Board b = GetBoard(email, boardName);
                            //if (b.GetTask(taskId).Assignee == email)
                            //{
                                try
                                {
                                    b.ChangeState(taskId, email);
                                    String msg = String.Format("task changed state Successfully in BuissnesLayer! to state :{0}", b.GetTask(taskId).GetState());
                                    log.Info(msg);
                                }
                                catch (Exception e)
                                {
                                    log.Warn(e.Message);
                                    throw new Exception(e.Message);
                                }
                            }
                            //else
                            //{
                            //    log.Warn("ONLY ASSIGNEE OF THE TASK CAN CHANGE ITS STATE");
                            //    throw new ArgumentException("ONLY ASSIGNEE OF THE TASK CAN CHANGE ITS STATE");
                            //}
                        else
                        {
                            log.Warn("task does not at columnOrdinal given");
                            throw new ArgumentException("task does not at columnOrdinal given");

                        }
                    }
                    else
                    {
                        log.Warn("value can not be null!!");
                        throw new ArgumentException("value can not be null!!");
                    }


                }
                catch (Exception e)
                {
                    log.Warn(e.Message);
                    throw new ArgumentException(e.Message);
                    //Response r = new Response(e.Message, false);
                    //return r.BadJson();
                }
            }
            else
            {
                log.Warn("user not logged in");
                throw new ArgumentException("user not logged in");
            }

        }
        /// <summary>
        /// This method assign a user from the board to a task.
        /// </summary>
        /// <param name="userEmailToAssign">Email of the user need to be assign to a task.</param>
        /// <param name="boardName">The name of the new board</param>
        /// <param name="userEmailAssigning">Email of the user assigning other user assign to a task.Must be logged in.</param> 
        /// <param name="taskId">The taskId of the task the userEmailAssigning will assigne </param>
        /// <returns>void, unless an error occurs .</returns>
        public void assignAssignee(string userEmailToAssign , string boardName ,int columnOrdinal, string userEmailAssigning ,int taskId )
        {
            if ((userController.IsLoggedIn(userEmailAssigning)))
            {
                try
                {
                    Board b = GetBoard(userEmailAssigning, boardName);
                    if (columnOrdinal == b.GetTask(taskId).GetState())//?????
                    {
                        if (columnOrdinal != Done && columnOrdinal != null)
                        {
                            if (b.IsInListOfJoiners(userEmailAssigning))
                            {
                                b.GetTask(taskId).EditAssignee(userEmailToAssign);
                                String msg = String.Format("task assignee assigned Successfully in BuissnesLayer! The assignee :{0}", userEmailToAssign);
                                log.Info(msg);
                            }
                            else
                            {
                                log.Warn("USER WHO IS NOT A MEMBER OF THE BOARD CAN NOT BE ASSIGNED TO TASK !");
                                throw new Exception("USER WHO IS NOT A MEMBER OF THE BOARD CAN NOT BE ASSIGNED TO TASK !");
                            }
                        }
                        else
                        {
                            log.Warn("NOT IN A COLUMN THAT THE USER CAN BE ASSIGN AT !");
                            throw new Exception("NOT IN A COLUMN THAT THE USER CAN BE ASSIGN AT !");
                        }
                    }
                    else
                    {
                        log.Warn("task does not exist at this ordinal");
                        throw new ArgumentException("task does not exist at this ordinal");
                    }
                }
                catch (Exception e)
                {
                    log.Warn(e.Message);
                    throw new ArgumentException(e.Message);
                }

            }
            else
            {
                log.Warn("user not logged in");
                throw new ArgumentException("user not logged in");
            }


        }
        /// <summary>
        /// This method add a new member to a board.
        /// </summary>
        /// <param name="userEmailJoiner">Email of the user added.</param>
        /// /// <param name="boardId">The id of the board</param>
        /// <returns>An empty response, unless an error occurs.</returns>
        public void joinBoard(int boardId,string userEmailJoiner)
        {
            try
            {
                string userEmailOwner = GetBoardById(boardId).GetOwner();
                string boardName = GetBoardById(boardId).name;
                if ((userController.IsLoggedIn(userEmailJoiner)))
                {
                    if (UserHasAnyBoard(userEmailJoiner))
                    {
                        if (!UserHasThisBoard(userEmailJoiner, boardName))
                        {
                            BoardsOfUsers[userEmailOwner][boardName].AddToJoinList(userEmailJoiner);
                            BoardsOfUsers[userEmailJoiner].Add(boardName, BoardsOfUsers[userEmailOwner][boardName]);
                            boardDTOMapper.AddUserToBoard(BoardsOfUsers[userEmailOwner][boardName].BoardID, userEmailJoiner);
                            String msg = String.Format("joined Board Successfully in BuissnesLayer! userEmailOwner = {0} the board :{1}", userEmailOwner, boardName);
                            log.Info(msg);
                        }
                        else
                        {
                            log.Warn("user rejoins a board they're already in");
                            throw new ArgumentException("user rejoins a board they're already in");
                        }
                    }
                    else
                    {
                        BoardsOfUsers[userEmailOwner][boardName].AddToJoinList(userEmailJoiner);
                        Dictionary<string, Board> b = new Dictionary<string, Board>();
                        b.Add(boardName, BoardsOfUsers[userEmailOwner][boardName]);
                        BoardsOfUsers[userEmailJoiner] = b;
                        boardDTOMapper.AddUserToBoard(BoardsOfUsers[userEmailOwner][boardName].BoardID, userEmailJoiner);
                        String msg = String.Format("joined Board Successfully in BuissnesLayer! userEmailOwner = {0} the board :{1}", userEmailOwner, boardName);
                        log.Info(msg);
                    }
                }
                else
                {
                    log.Warn("user not logged in");
                    throw new ArgumentException("user not logged in");
                }

            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
            }
            
        }
        /// <summary>
        /// This method remove a member from a board.
        /// </summary>
        /// <param name="userEmailOwner">Email of the user owner.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="userEmailLeaving">Email of the user removed.</param>
        /// <returns>An empty response, unless an error occurs.</returns>
        public void leaveBoard(int boardId, string userEmailLeaving)
        {
            try
            {
                string userEmailOwner = GetBoardById(boardId).GetOwner();
                string boardName = GetBoardById(boardId).name;
                if ((userController.IsLoggedIn(userEmailLeaving)))
                {
                    if (UserHasThisBoard(userEmailLeaving, boardName))
                    {
                        if (userEmailLeaving!=userEmailOwner)
                        {
                            BoardsOfUsers[userEmailOwner][boardName].leaveTasks(userEmailLeaving); // all joiner take become unAssigned
                            BoardsOfUsers[userEmailOwner][boardName].DeleteFromJoinList(userEmailLeaving);
                            BoardsOfUsers[userEmailLeaving].Remove(boardName); // delete to userEmailLeaving from boards
                            this.boardDTOMapper.RemoveUserFromBoard(BoardsOfUsers[userEmailOwner][boardName].BoardID, userEmailLeaving);
                            String msg = String.Format("userEmail Left Successfully in BuissnesLayer! userEmailOwner = {0} the board :{1}", userEmailLeaving, boardName);
                            log.Info(msg);
                        }
                        else
                        {
                            log.Warn("OWNER CAN'T LEAVE HIS OWN BOARD!");
                            throw new ArgumentException("OWNER CAN'T LEAVE HIS OWN BOARD!");
                        }

                    }
                    else
                    {
                        log.Warn("user doesn't have that board");
                        throw new ArgumentException("user doesn't have that board");
                    }
                }
                else
                {
                    log.Warn("user not logged in");
                    throw new ArgumentException("user not logged in");
                }
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
        /// <param name="userEmailOwner">Email of the current owner. Must be logged in</param>
        /// <param name="userEmailFutureOwner">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns> void, unless an error occurs </returns>
        public void  switchOwnership(string userEmailOwner, string boardName, string userEmailFutureOwner)
        {
            try
            {
                if ((userController.IsLoggedIn(userEmailOwner)))
                {
                    if (isOwnerOfAnyBoard(userEmailOwner))
                    {
                        if (ownerBoards[userEmailOwner].Contains(boardName))
                        {
                            if (BoardsOfUsers[userEmailOwner][boardName].IsInListOfJoiners(userEmailFutureOwner))
                            {
                                if (isOwnerOfAnyBoard(userEmailFutureOwner))// checks if userEmailFutureOwner is owner of other board
                                {
                                    if (!ownerBoards[userEmailFutureOwner].Contains(boardName))
                                    {
                                        boardDTOMapper.ChangeOwnership(userEmailFutureOwner, GetBoard(userEmailOwner, boardName).BoardID); // Needs to happen before because we're using GetBoard
                                        BoardsOfUsers[userEmailOwner][boardName].SetOwner(userEmailFutureOwner);
                                        ownerBoards[userEmailFutureOwner].Add(boardName);
                                        ownerBoards[userEmailOwner].Remove(boardName);
                                        // if (isOwnerOfAnyBoard(userEmailFutureOwner)) // checks if userEmailFutureOwner is owner of other board
                                        // {
                                        //     ownerBoards[userEmailFutureOwner].Add(boardName);
                                        //     ownerBoards[userEmailOwner].Remove(boardName);
                                        // }
                                        // else
                                        // {
                                        //     List<string> listBoard = new List<string>();
                                        //     listBoard.Add(boardName);
                                        //     ownerBoards.Add(userEmailFutureOwner, listBoard);
                                        //     ownerBoards[userEmailOwner].Remove(boardName);
                                        // }

                                    }
                                    else
                                    {
                                        log.Warn("USER CANT HAVE TWO BOARDS WITH THE SAME NAME");
                                        throw new ArgumentException("USER CANT HAVE TWO BOARDS WITH THE SAME NAME");
                                    }


                                }
                                else
                                {
                                    boardDTOMapper.ChangeOwnership(userEmailFutureOwner,
                                        GetBoard(userEmailOwner, boardName).BoardID); // Needs to happen before because we're using GetBoard
                                    List<string> listBoard = new List<string>();
                                    listBoard.Add(boardName);
                                    ownerBoards.Add(userEmailFutureOwner, listBoard);
                                    ownerBoards[userEmailOwner].Remove(boardName);

                                }
                                String msg = String.Format("Transfer the Ownership Successfully in BuissnesLayer!  new Owner userEmail = {0} of board :{1}", userEmailFutureOwner, boardName);
                                log.Info(msg);

                            }
                            else
                            {
                                log.Warn("USER DOES NOT A MEMBER OF THIS BOARD");
                                throw new ArgumentException("USER DOES NOT A MEMBER OF THIS BOARD");
                            }

                        }
                        else
                        {
                            log.Warn("not the owner of this board!");
                            throw new ArgumentException("not the owner of this board!");
                        }
                    }
                    else
                    {
                        log.Warn("not owner of any board");
                        throw new ArgumentException("not owner of any board");
                    }
                    
                }
                else
                {
                    log.Warn("user not logged in");
                    throw new ArgumentException("user not logged in");
                }

            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
            }
        }
        // /// <summary>
        // /// This method changes board owner.
        // /// </summary>
        // /// <returns>void.</returns>
        // public void changeOwner(string currntUserEmail,string nextUserEmail , string boardName)
        // {
        //     try
        //     {
        //         if((userController.IsLoggedIn(currntUserEmail)))
        //         {
        //             if (BoardsOfUsers[currntUserEmail][boardName].IsInListOfJoiners(nextUserEmail))
        //             {
        //                 BoardsOfUsers[currntUserEmail][boardName].SetOwner(nextUserEmail);
        //                 List<string> value = ownerBoards[currntUserEmail];
        //                 ownerBoards.Remove(currntUserEmail);
        //                 ownerBoards.Add(nextUserEmail,value);
        //             }
        //         }
        //         else {
        //             throw new ArgumentException("user not logged in");
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         throw new ArgumentException(e.Message);
        //     }
        // }

        /// <summary>
        /// This method returns all the In progress tasks the user assigned to.
        /// </summary>
        /// <returns>Response with a list of the in progress tasks, unless an error occurs .</returns>
        public List<Task> GetAllInPrograss(string userEmail)
        {
            try
            {
                if (userController.IsLoggedIn(userEmail))
                {
                    if (BoardsOfUsers.ContainsKey(userEmail))
                    {
                        Dictionary<string, Board> boards = BoardsOfUsers[userEmail];
                        List<Task> taskInProgList = new List<Task>();
                        foreach (var item in boards.Keys)
                        {
                            taskInProgList.AddRange(boards[item].GetInProgressByAssignee(userEmail));
                        }
                        return taskInProgList;
                    }
                    else
                    {
                        List<Task> taskInProgList = new List<Task>();
                        return taskInProgList;
                    }

                    String msg = String.Format("got InProgress list Successfully in BuissnesLayer! userEmail = {0} ", userEmail);
                    log.Info(msg);
                }
                else
                {
                    log.Warn("user not logged in");
                    throw new ArgumentException("user not logged in");
                }
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
            }
            
        }
        /// <summary>
        /// This method removes a board to the specific user.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>void, unless an error occurs </returns>
        public void DeleteBoard(string userEmail, string boardName)
        {
            try
            {
                if (userController.IsLoggedIn(userEmail))
                {
                    if (UserHasThisBoard(userEmail, boardName))
                    {
                        if (BoardsOfUsers[userEmail][boardName].GetOwner() == userEmail)
                        {
                            int IDtoRemove = GetBoard(userEmail, boardName).BoardID;
                            this.boardById.Remove(GetBoard(userEmail, boardName).BoardID);
                            this.BoardsOfUsers[userEmail].Remove(boardName);
                            this.ownerBoards[userEmail].Remove(boardName);
                            // Logically speaking - boards are recognized by ID.
                            // However, the GradingService recognizes them by
                            // owner email and board name as a double key. 
                            // I believe that ID's 

                            this.boardDTOMapper.DeleteBoard(userEmail, boardName, IDtoRemove);
                            this.BoardsOfUsers[userEmail].Remove(boardName);
                            this.ownerBoards[userEmail].Remove(boardName);
                            String msg = String.Format("Deleted Successfully in BuissnesLayer! userEmail = {0} deleted board :{1}", userEmail, boardName);
                            log.Info(msg);
                        }
                        else
                        {
                            log.Warn("THIS USER ISN'T THE OWNER OF THE BOARD ! ");
                            throw new ArgumentException("THIS USER ISN'T THE OWNER OF THE BOARD ! ");
                        }

                    }
                    else
                    {
                        log.Warn("BOARD IS NOT EXIST AT THIS USER ! ");
                        throw new ArgumentException("BOARD IS NOT EXIST AT THIS USER ! ");
                    }

                }
                else
                {
                    log.Warn("user not logged in");
                    throw new ArgumentException("user not logged in");
                }
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
            }
        }

        /// <summary>
        /// This method get a specific board to the specific user.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>Board, unless an error occurs .</returns>
        public Board GetBoard(string userEmail, string boardName)
        {
            if (userController.IsLoggedIn(userEmail))
            {
                if (UserHasThisBoard(userEmail, boardName))
                {
                    return this.BoardsOfUsers[userEmail][boardName];
                    String msg = String.Format("Got board Successfully in BuissnesLayer! userEmail = {0}  board ={1}", userEmail, boardName);
                    log.Info(msg);
                }
                else
                {
                    log.Warn("BOARD IS NOT EXIST AT THIS USER ! ");
                    throw new ArgumentException("BOARD IS NOT EXIST AT THIS USER ! ");
                }

            }
            else
            {
                log.Warn("user not logged in");
                throw new ArgumentException("user not logged in");
            }

        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with  a list of the column's tasks, unless an error occurs </returns>
        public List<Task> GetColum(string email, string boardName, int columnOrdinal)
        {
            try
            {
                return GetBoard(email, boardName).GetColList(columnOrdinal, email); // Very important to send with the email.
                String msg = String.Format("GetColum Successfully in BuissnesLayer! columnOrdinal = {0}  board ={1}", columnOrdinal, boardName);
                log.Info(msg);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>The string "{}", unless an error occurs </returns>
        public void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                GetBoard(email, boardName).SetMaxTask(limit, columnOrdinal);
                boardDTOMapper.ChangeColumnLimit(GetBoard(email, boardName).BoardID, columnOrdinal, limit);
                String msg = String.Format("set LimitColumn Successfully in BuissnesLayer! columnOrdinal = {0}  board ={1}", columnOrdinal, boardName);
                log.Info(msg);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new Exception(e.Message);
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
                return GetBoard(email, boardName).GetNameOrdinal(columnOrdinal);
                String msg = String.Format("GetColumnName Successfully in BuissnesLayer! columnOrdinal = {0}  board ={1}", columnOrdinal, boardName);
                log.Info(msg);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with column limit value, unless an error occurs </returns>
        public int GetColumnLim(string email, string boardName, int columnOrdinal)
        {
            try
            {
                return GetBoard(email, boardName).GetMaxTask(columnOrdinal);
                String msg = String.Format(" GetColumnLim Successfully in BuissnesLayer! columnOrdinal = {0}  board ={1}", columnOrdinal, boardName);
                log.Info(msg);
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// This method get a specific Task to the specific user.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        /// <param name="taskId">The id of new task</param>
        /// <returns>Task, unless an error occurs .</returns>
        public Task GetTask(string email, string boardName, int taskId)
        {
            try
            {
                String msg = String.Format("Got Task Successfully in boardcontroller BuissnesLayer!");
                    log.Info(msg);
                    return GetBoard(email, boardName).GetTask(taskId);
                
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
            }
        }
        /// <summary>
        /// This method get a specific Task to the specific user.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        /// <param name="taskId">The id of new task</param>
        /// <returns>Task, unless an error occurs .</returns>
        public Task GetTask(string email, string boardName, int taskId, int columnOrdinal)
        {
            try
            {
                if(columnOrdinal != BacklogState && columnOrdinal != inProgressState && columnOrdinal != Done)
                {
                    log.Warn("got column ordinal different the allowed parameters!");
                    throw new ArgumentException("got column ordinal different the allowed parameters!");

                }
                else
                {
                    String msg = String.Format("Got Task Successfully in BuissnesLayer!");
                    log.Info(msg);
                    return GetBoard(email, boardName).GetTask(taskId);
                }
            }
            catch (Exception e)
            {
                log.Warn(e.Message);
                throw new ArgumentException(e.Message);
            }
        }
        /// <summary>
        /// This method LoadData to boardDTO.
        /// </summary>
        /// <returns>void </returns>
        public void LoadData()
        {
            // this.boardsList = 
            List<BoardDTO> boardDTOs = this.boardDTOMapper.LoadBoards();
            foreach (var boardDTO in boardDTOs)
            {
                boardDTO.LoadBoard();
                LoadBoard(boardDTO.Owner, boardDTO.Name, boardDTO);
                // Create Board object
                // Load info based on boardDTOs (don't forget board_count)
            }
            String msg = String.Format("LoadData Successfully in BuissnesLayer!");
            log.Info(msg);
        }
        /// <summary>
        /// This method DeleteAllData to boardDTO.
        /// </summary>
        /// <returns>void </returns>
        public void DeleteAllData()
        {
            this.boardDTOMapper.DeleteAllData();
            this.BoardsOfUsers.Clear();
            this.ownerBoards.Clear();
            this.boardById.Clear();
            String msg = String.Format(" DeleteAllData Successfully in BuissnesLayer!");
            log.Info(msg);
        }
      

    }
}
