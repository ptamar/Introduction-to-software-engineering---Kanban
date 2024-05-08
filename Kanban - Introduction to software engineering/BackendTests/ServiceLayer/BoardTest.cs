
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using IntroSE.Kanban.Backend.Buissnes_Layer;
using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendTests.ServiceLayer
{
    internal class BoardTest
    {
        private BoardService _boardService;
        public BoardTest(BoardService BS)
        {
            this._boardService = BS;
        }

        /// <summary>
        /// This method tests a valid creation of a new boardService in the system according to requirement 9.
        /// </summary>
        [TestMethod()]
        public void ValidCreateBoardTest()
        {

            Response r = new Response(null, true);
            string email = "johndoe@gmail.com";
            string boardName = "test2";
            Assert.AreEqual(_boardService.CreateBoard(boardName, email), ToJson.toJson(r));

        }

        /// <summary>
        /// This method tests a invalid creation of a new boardService in the system according to requirement 9.
        /// </summary>
        [TestMethod()]
        public void InvalidCreateBoardTest()
        {
            Response r = new Response("User does not exist", false);
            string email = "test@gmail.com";
            string boardName = "testName";
            Assert.AreEqual(_boardService.CreateBoard(boardName, email), r.BadJson());


        }
        /// <summary>
        /// This method tests a invalid creation of a new boardService with the same name of a board that already exist  according to requirement  6.
        /// </summary>
        [TestMethod()]
        public void InvalidCreateBoardTest2()
        {
            Response r = new Response("USER CANNOT CREATE A THIS BOARD! USER HAS A BOARD WITH THIS NAME ALREADY", false);
            string email = "tamar@gmail.com";
            string boardName = "testName";
            _boardService.CreateBoard(boardName, email);
            Assert.AreEqual(_boardService.CreateBoard(boardName, email), r.BadJson());

        }
        /// <summary>
        /// This method tests a valid add of a new Task to a boardService in the system according to requirement  12.
        /// check the task was added at the right place(backlog only), and its name is different from the other tasks at the boardService.
        /// </summary>
        [TestMethod()]
        public void AddValidTaskTest()
        {
            
            string email = "tamar@gmail.com";
            string boardName = "To do list";
            string title = "HW";
            string description = "EX3";
            DateTime dueDate = new DateTime(2025,05,10);
            Response r = new Response(null, email);
            Assert.AreEqual(_boardService.AddTask(email, boardName, title, description, dueDate), ToJson.toJson(r));
            Console.WriteLine("wwoohooo added task succsusfully");
            Assert.AreEqual(_boardService.AddTask(email, boardName, "ss", description, new DateTime(2025, 05, 22)), ToJson.toJson(r));
            Console.WriteLine("wwoohooo added task2 succsusfully");
        }
        
        /// <summary>
        /// This method tests a invalid add of a new Task to a boardService in the system according to requirement  12.
        /// check the task was added at the right place(backlog only) and has invalid email.
        /// </summary>
        [TestMethod()]
        public void AddInvalidTaskTest2()
        {
            string email = "wrong@gmail.com";
            string password = "1234";
            string boardName = "testName";
            string title = "HW";
            string description = "EX3";
            DateTime dueDate = new DateTime(14 / 07 / 2025);
            try
            {
                _boardService.AddTask(email, boardName, title, description, dueDate);

            }
            catch (Exception e)
            {
               
                Assert.AreEqual(e.Message, "User does not exist");
            }

           
        }
        /// <summary>
        /// This method tests a valid change  state of a TaskService from one state to the next in the system according to requirement  13.
        /// check the task mooved for the right next test.
        /// </summary>
        [TestMethod]
        public void ValidNextStateTest()
        {
            string email = "tamar@gmail.com";
            string boardName = "testName";
            Response r = new Response(null, true);

            Assert.AreEqual(_boardService.NextState(email, boardName, 0, 0), r.OKJson());


        }
        /// <summary>
        /// This method tests a invalid change state of a TaskService from one state to the next in the system according to requirement  13.
        /// check the task moved for the right next test and invalid email user.
        /// </summary>
        [TestMethod]
        public void InvalidNextStateTest()
        {
            string email = "wrong@gmail.com";
            string boardName = "testName";
            try
            {
                _boardService.NextState(email, boardName, 0, 0);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "User does not exist");
            }
            

        }
        /// <summary>
        /// This method tests a invalid change  state of a TaskService from one state to the next in the system according to requirement  13.
        /// check the task moved for the right next test and invalid taskId.
        /// </summary>
        [TestMethod]
        public void InvalidNextStateTest2()
        {
            string email = "tamar@gmail.com";
            string boardName = "testName";
            try
            {

                _boardService.NextState(email, boardName, 0, 0);

            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "TASK Does not exist! ");
            }
        }
        /// <summary>
        /// This method tests a invalid change  state of a TaskService from one state to the next in the system according to requirement  13.
        /// check the task can not be moved over Done state.
        /// </summary>
        [TestMethod]
        public void InvalidNextStateTest3()
        {
            string email = "tamar@gmail.com";
            string boardName = "testName";
            int taskId = 0;

            _boardService.NextState(email, boardName, 0, 0);
            try
            {
                _boardService.NextState(email, boardName, 0, 0);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "TASK STATE CAN'T BE CHANGED! ALREADY AT DONE ");
            }


        }
        // <summary>
        // This method tests a valid deletion of a  boardService in the system according to requirement 9.
        // </summary>
        [TestMethod]
        public void ValidDeleteBoardTest()
        {
            string email = "tamar@gmail.com";
            string boardName = "testName";
            Response r = new Response(null, true);
            Assert.AreEqual(_boardService.DeleteBoard(boardName, email), r.OKJson());

        }
        /// <summary>
        /// This method tests a invalid deletion of a  boardService in the system according to requirement 9.
        /// </summary>
        [TestMethod]
        public void InvalidDeleteBoardTest()
        {
            Response r = new Response("BOARD IS NOT EXIST AT THIS USER ! ", false);
            string email = "tamar@gmail.com";
            string boardName = "NotExist";
            try
            {
                _boardService.DeleteBoard(boardName, email);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "BOARD IS NOT EXIST AT THIS USER ! ");
            }
           
        }
        /// <summary>
        /// This method tests a valid get of InProgress tasks of a user in the system according to requirement 16.
        /// </summary>
        [TestMethod()]
        public void ValidInProgress()
        {
            string email = "tamar@gmail.com";
            Response r = new Response(_boardService.boardController.GetAllInPrograss(email));
            Assert.AreEqual(_boardService.InProgress(email), ToJson.toJson(r));

        }

        /// <summary>
        /// This method tests a valid get of InProgress tasks of a user in the system according to requirement 16.
        /// A successful test returns all InProgress tasks that the user in assigned to.
        /// </summary>
        [TestMethod()]
        public void ValidInProgress_2()
        {
            string email = "tamar@gmail.com";
            Response r = new Response(_boardService.boardController.GetAllInPrograss(email));
            Assert.AreEqual(_boardService.InProgress(email), ToJson.toJson(r));

        }

        /// <summary>
        /// This method tests a invalid get of InProgress tasks of a user in the system according to requirement 16.
        /// </summary>
        [TestMethod()]
        public void InvalidInProgress()
        {
            Response r = new Response("User does not exist", false);
            string email = "test@gmail.com";
            try
            {
                _boardService.InProgress(email);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "User does not exist");

            }

        }
    
        /// <summary>
        /// This method tests a valid get of a task column of certain board user in the system .
        /// </summary>
        [TestMethod()]
        public void ValidGetColum()
        {
            string email = "tamar@gmail.com";
            string boardName = "testName";
            int columnOrdinal = 0;
            Assert.AreEqual(_boardService.GetColum(email, boardName, columnOrdinal), _boardService.GetColum(email, boardName, columnOrdinal));
        }

        /// <summary>
        /// This method tests a invalid get of a task column of certain board user in the system .
        /// check the method throws exception when column does not exist.
        /// </summary>
        [TestMethod()]
        public void InvalidGetColum()
        {

            string email = "tamar@gmail.com";
            string boardName = "testName";
            int columnOrdinal = 3;
            try
            {
                _boardService.GetColum(email, boardName, columnOrdinal);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "this column state does not exist!");

            }

        }
        /// <summary>
        /// This method tests a valid set of a column limit of certain board user in the system according to requirement  10.
        /// </summary>
        [TestMethod]
        public void ValidLimitColumn()
        {
            string email = "tamar@gmail.com";
            string boardName = "testName";
            int limit = 10;
            int columnOrdinal = 2;
            Response r = new Response(null);
            Assert.AreEqual(_boardService.LimitColumn(email, boardName, columnOrdinal,limit), ToJson.toJson(r));

        }
        /// <summary>
        /// This method tests a valid set of a column limit of certain board user in the system according to requirement  10.
        /// </summary>
        [TestMethod]
        public void ValidLimitColumn2()
        {
            string email = "tamar@gmail.com";
            string boardName = "testName";
            int limit = 10;
            int columnOrdinal = 1;
            Response r = new Response(null);
            Assert.AreEqual(_boardService.LimitColumn(email, "To do list", columnOrdinal, limit), ToJson.toJson(r));
        }
        /// <summary>
        /// This method tests a invalid set of a column limit of certain board user in the system according to requirement  10.
        /// check the method throws exception when column does not exist.
        /// </summary>
        [TestMethod]
        public void InvalidLimitColumn()
        {
            string email = "tamar@gmail.com";
            string boardName = "testName";
            int limit = 10;
            int columnOrdinal = 4;
            try
            {
                _boardService.LimitColumn(email, boardName, columnOrdinal, limit);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "this column state does not exist!");
            }

        }
        /// <summary>
        /// This method tests a invalid change  state of a TaskService from one state to the next in the system.
        /// check the task moved for the right next test and invalid taskId.
        /// </summary>
        [TestMethod]
        public void ValidGetColumnName()
        {
            string email = "tamar@gmail.com";
            string boardName = "testName";
            int columnOrdinal = 0;
            string colVal = _boardService.boardController.GetBoard(email, boardName).GetNameOrdinal(columnOrdinal);
            Response r = new Response(null, colVal);
            Assert.AreEqual(_boardService.GetColumnName(email, boardName, columnOrdinal), "backlog");
        }
        /// <summary>
        /// This method tests a invalid change
        ///state of a TaskService from one state to the next in the system .
        /// check the task can not be moved over Done state.
        /// </summary>
        [TestMethod]
        public void InvalidGetColumnName()
        {
            string email = "tamar@gmail.com";
            string boardName = "testName";
            int columnOrdinal = 3;
            try
            {
                _boardService.GetColumnName(email, boardName, columnOrdinal);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "this column state does not exist!");

            }
        }
        // <summary>
        // This method tests a valid deletion of a  boardService in the system according to requirement 9.
        // </summary>
        [TestMethod]
        public void ValidGetColumnLimit()
        {
            string email = "tamar@gmail.com";
            string boardName = "testName";
            int columnOrdinal = 2;
            int colVal = _boardService.boardController.GetBoard(email, boardName).GetMaxTask(columnOrdinal);
            Response r = new Response(null, colVal);
            Assert.AreEqual(_boardService.GetColumnLimit(email, boardName,columnOrdinal),"-1");

        }
        /// <summary>
        /// This method tests a invalid deletion of a  boardService in the system according to requirement 9.
        /// </summary>
        [TestMethod]
        public void InvalidGetColumnLimit()
        {
            Response r = new Response("this column state does not exist!", false);
            string email = "tamar@gmail.com";
            string boardName = "testName";
            int columnOrdinal = 4;
            try
            {
                _boardService.GetColumnLimit(email, boardName, columnOrdinal);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "this column state does not exist!");
            }
            
        }
        /// <summary>
        /// Tests if a board has an ID
        /// </summary>
        public void ValidGetBoardById()
        {
            string email = "johndoe@gmail.com";
            string boardName = "To do list";
            Assert.AreEqual(_boardService.boardController.GetBoardById(0), _boardService.boardController.GetBoard(email,boardName));
        }
        /// <summary>
        /// Tests if a board has an owner
        /// </summary>
        public void GetOwner()
        {
            string email = "johndoe@gmail.com";
            string boardName = "To do list";
            Assert.AreEqual(_boardService.boardController.GetBoard(email,boardName).GetOwner(), "johndoe@gmail.com");
        }
        /// <summary>
        /// Tests the successful deletion of a board
        /// </summary>
        public void ValidDeleteBoard()
        {
            string email = "johndoe@gmail.com";
            string boardName = "To do list";
            Response r = new Response(null);
            Assert.AreEqual(_boardService.DeleteBoard(boardName,email),ToJson.toJson(r));
        }
        /// <summary>
        /// Tests the deletion of a board with a user that not related to this board. 
        /// </summary>
        public void InvalidDeleteBoard()//run this test before join this member to the board 
        {
            string boardName = "To do list";
            Assert.AreEqual(_boardService.DeleteBoard(boardName, "tamar@gmail.com"), " BOARD IS NOT EXIST AT THIS USER ! ");
        }
        /// <summary>
        /// Tests a deletion attempt of a board by a user who is not the owner
        /// </summary>
        public void InvalidDeleteBoard_2()//run this test after join this member to the board 
        {
            string boardName = "To do list";
            Assert.AreEqual(_boardService.DeleteBoard(boardName, "tamar@gmail.com"),
                "THIS USER ISN'T THE OWNER OF THE BOARD !");
        }


        /// <summary>
        /// Tests the successful enrollment of a user to a board
        /// </summary>
        public void JoinBoardSuccessfully()
        {
            Response r = new Response(null);

            Assert.AreEqual(_boardService.JoinBoard(1, "tamar@gmail.com"),
                ToJson.toJson(r));
        }

        /// <summary>
        /// Tests an invalid enrollment to a board which doesn't exist.
        /// </summary>
        public void JoinBoardUnsuccessfully()
        {
            Assert.AreEqual(_boardService.JoinBoard(2, "tamar@gmail.com"), "THIS BOARD DOES NOT EXSIT");
        }

        /// <summary>
        /// Tests an invalid enrollment to a board that the user us a member of already
        /// </summary>
        public void JoinBoardUnsuccessfully_2()
        {
            Assert.AreEqual(_boardService.JoinBoard(1, "tamar@gmail.com"), "user already joined that board");
        }

        /// <summary>
        /// Tests a valid quitting of a board
        /// </summary>
        public void LeaveBoardSuccessfully()
        {
            Response r = new Response(true);
            Assert.AreEqual(_boardService.LeaveBoard(1, "tamar@gmail.com"), ToJson.toJson(r));
        }

        /// <summary>
        /// Tests an attempt to leave a board by a user who was not a member
        /// </summary>
        public void LeaveBoardUnsuccessfully()
        {
            Assert.AreEqual(_boardService.LeaveBoard(1, "itay@gmail.com"), "user doesn't have that board");
        }

        /// <summary>
        /// Tests an attempt to leave the board by the board owner - which is defined to be illegal
        /// </summary>
        public void LeaveBoardUnsuccessfully_2()
        {
            Assert.AreEqual(_boardService.LeaveBoard(1, "johndoe@gmail.com"), "OWNER CAN'T LEAVE HIS OWN BOARD!");

        }
        /// <summary>
        /// Tests an attempt to leave a board that does not exist
        /// </summary>
        public void LeaveBoardUnsuccessfully_3()
        {
            Assert.AreEqual(_boardService.LeaveBoard(2, "johndoe@gmail.com"), "OWNER CAN'T LEAVE HIS OWN BOARD!");

        }

        /// <summary>
        /// Tests the valid exchange of ownership
        /// </summary>
        public void ChangeOwnerSuccessfully()
        {
            Response r = new Response(null);
            Assert.AreEqual(_boardService.TransferOwnership("johndoe@gmail.com", "tamar@gmail.com", "To do list"), ToJson.toJson(r));

        }

        /// <summary>
        /// Tests an attempt to assign an owner who is not currently a board member
        /// </summary>
        public void ChangeOwnerUnsuccessfully()
        {
            Assert.AreEqual(_boardService.TransferOwnership("johndoe@gmail.com", "tamar@gmail.com", "To do list"), "USER DOES NOT A MEMBER OF THIS BOARD");

        }

        /// <summary>
        /// Tests an attempt to "steal" a board ownership - ie. a user who is not the owner tries to assign themselves as the owner.
        /// </summary>
        public void ChangeOwnerUnsuccessfully_2()
        {
            Assert.AreEqual(_boardService.TransferOwnership("johndoe@gmail.com", "itay@gmail.com", "To do list"), "user not logged in");

        }
        /// <summary>
        /// Tests an attempt to switch owner to board that does not exist
        /// </summary>
        public void ChangeOwnerUnsuccessfully_3()
        {
            Assert.AreEqual(_boardService.TransferOwnership("johndoe@gmail.com", "itay@gmail.com", "To do "), "user not logged in");

        }
        /// <summary>
        /// Tests an attempt to switch owner to board with a user that not an owner of any board.
        /// </summary>
        public void ChangeOwnerUnsuccessfully_4()
        {
            Assert.AreEqual(_boardService.TransferOwnership("tamar@gmail.com", "itay@gmail.com", "To do list"), "not owner of any board");

        }

        // /// <summary>
        // /// This methods checks that a task indeed has a user assigned to it.
        // /// </summary>
        // public void AddTask_2()
        // {
        //     Assert.AreEqual(_boardService.AddTask("tamar@gmail.com"), "not owner of any board");
        // }

        /// <summary>
        /// This methods checks that u can actuallu assigne user mamber a the board sucessfully.
        /// </summary>
        public void AssignTaskSuccessfully(string email, string boardName)
        {
            Response r = new Response(null, email);
            Assert.AreEqual(_boardService.AssignTask(email, boardName, 0, "itay@gmail.com", 1), ToJson.toJson(r));
        }

        /// <summary>
        /// This methods checks that a task indeed has a user assigned to it.
        /// </summary>
        public void AssignTaskUnSuccessfully()
        {
            Response r = new Response(null, "tamar@gmail.com");
            Assert.AreEqual(_boardService.AssignTask("tamar@gmail.com", "To do list", 0, "itay@gmail.com", 1), ToJson.toJson(r));

        }


    }
}

