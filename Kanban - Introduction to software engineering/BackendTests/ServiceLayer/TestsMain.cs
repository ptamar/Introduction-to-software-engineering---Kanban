//using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntroSE.Kanban.Backend.Buissnes_Layer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests.ServiceLayer
{
    // [TestClass()]
    internal class TestsMain
    {
        private UserController userController;
        public UserService userService;
        public BoardService boardService;
        public TaskService taskService;
        public TestsMain()
        {
            this.userController = new UserController();
            this.userService = new UserService(this.userController);
            this.boardService = new BoardService(this.userController);
            this.taskService = new TaskService(boardService.boardController);
            string email = "test@gmail.com";
            string password = "Ai123456";
            string boardName = "testName";
            string title = "HW";
            string description = "EX3";
            DateTime dueDate = new DateTime(14 / 07 / 2025);
            Console.WriteLine("yayy");
            // userService.CreateUser(email, password);
            // userService.Login(email, password);
            //
            // boardService.CreateBoard(boardName, email);
            // boardService.AddTask(email, boardName, title, description, dueDate);
            //TaskTests tests = new TaskTests(taskService, userService, boardService);
        }
        // [TestMethod()]
        static void Main(string[] args)
        {
            ConsoleColor c = ConsoleColor.Black;
            Console.BackgroundColor = c;
            Console.ForegroundColor = ConsoleColor.White;
            // Display the number of command line arguments.
            UserController userController = new UserController();
            UserService userService = new UserService(userController);
            BoardService boardService = new BoardService(userController);
            TaskService taskService = new TaskService(boardService.boardController);
            UserTests userTests = new UserTests(userController, userService); 
            GradingService grading = new GradingService();
            TaskTests taskTests = new TaskTests(taskService, userService, boardService, grading);
            string email1 = "johndoe@gmail.com";
            string password = "Hash123";
            string email2 = "johnDOe2@gmail.com";
            string password2 = "Bad_Password!#!@6";
            string email3 = "johndoe3@gmail.com";
            string password3 = "Bad_Password78@@;";
            string boardName = "testName";
            string title = "HW";
            string description = "EX3";
            DateTime dueDate = new DateTime(2025, 6, 15);
            DateTime newDueDate = new DateTime(2026, 8, 14);
            userService.DeleteAllData();
            boardService.DeleteAllData();
            
           
            userTests.DeleteData();
            userTests.createUserTest();
            userTests.invalidUserLoginTest();
            userTests.invalidUserCreation();
            userTests.invalidLoginTest_2();
            userTests.invalidUserCreation_2();
            userTests.logoutTest();
            userTests.invalidLogoutTest();
            userTests.validUserLoginTest();
            userTests.invalidUserCreation_3();
            
            userService.CreateUser("j@Gmail.com", "Hash123");
            userService.CreateUser("j.ohndoe@Gmail.com", "Hash123");
            userService.CreateUser("j@Gmail.", "Hash123");
            userService.CreateUser("johndoe@", "Hash123");
            userService.CreateUser("johndoegmailcom", "Hash123");
            userService.CreateUser("johndoe@gmail.com", "ash123");
            userService.CreateUser("johndoe@gmail.com", "hasher");




            
            BoardTest boraTest = new BoardTest(boardService);

             
            userTests.validUserLoginTest();
            userService.CreateUser("johndoe3@gmail.com", "Hash123");
            userService.CreateUser("sa@aa.co", "Kkk666");
            userTests.validUserLoginTest();
            userTests.invalidUserLoginTest();
            
            userService.Login("johndoe@gmail.com", "Hash123");
            boardService.CreateBoard("To do list", "johndoe@gmail.com");
            grading.Login("johndoe2@gmail.com", "Hash123");
            Console.WriteLine(grading.AddBoard("johndoe2@gmail.com","To do list"));

            //boardService.AddTask("johndoe@gmail.com", "To do list2", "test", "ssa", dueDate);
            grading.AddTask("johndoe2@gmail.com", "To do list", "test", "ssa", dueDate);
            
            Console.WriteLine(grading.UpdateTaskTitle("johndoe2@gmail.com", "To do list", 0,1, null));

            //taskService.EditDescription("johndoe2@gmail.com", "To do list", 1, "Hello");
            //taskService.EditDueDate("johndoe2@gmail.com", "To do list", 1, newDueDate);
            /* Lessons I've learned today -
             1) You can't call methods in the gradingService after 
            instantiating users\boards in the userService\boardService.
            These are like 2 different environments. This may cause bugs in the future 
            and perhaps even creates bugs rn.
            Perhaps we do need to implement a ServiceFactory after all :(.
            2) For some reason it seems as though you can't use column names as parameters 
            when in command.Parameters - take note of that and insert column\table names 
            as variables instead of parameters. - Needs confirmation.
            3) We do need to start calling methods in the TestsMain using actual
            tests. Without them we miss some crucial sanity checks which consume
            a lot of time in debugging.
              */
            //boardService.LimitColumn("johndoe2@gmail.com", "To do list", 1, 17);

            

            boraTest.ValidGetBoardById();
            boraTest.GetOwner();
            userService.CreateUser("tamar@gmail.com", "Hash123");
            userService.Login("tamar@gmail.com", "Hash123");
            // boraTest.InvalidDeleteBoard();
            // boraTest.JoinBoardSuccessfully();
            // boraTest.AddValidTaskTest();
            // boraTest.ValidLimitColumn2();
            boardService.CreateBoard("To do list", "tamar@gmail.com");
            // boraTest.JoinBoardSuccessfully();// checks if  the user can join to board has the same name as a board he got.
            //boraTest.JoinBoardUnsuccessfully_2();
            // boraTest.JoinBoardUnsuccessfully();
            // boraTest.ChangeOwnerSuccessfully();
            // boraTest.LeaveBoardSuccessfully();
            userController.CreateUser("itay@gmail.com", "Hash123");
            userController.Login("itay@gmail.com", "Hash123");
            boardService.boardController.joinBoard(1, "itay@gmail.com");
            // The following throws exceptions:
            // boraTest.AssignTaskSuccessfully("itay@gmail.com", "To do list");
            // taskTests.IsAssigneeTest("itay@gmail.com", "To do list");
            // boraTest.ChangeOwnerSuccessfully();

            
            // boraTest.ChangeOwnerSuccessfully(); \\ owner loged in and user is a member -> successfully
            // boraTest.LeaveBoardSuccessfully();//tamar leave the board
            // boraTest.ChangeOwnerUnsuccessfully(); //tamar is not a nenber of this board
            // userService.logout("johndoe@gmail.com");// the owner of to do board loged out
            // boraTest.ChangeOwnerUnsuccessfully_2();// the member of the board try to "steal" the ownership
            // boraTest.LeaveBoardUnsuccessfully();
            // boraTest.ChangeOwnerUnsuccessfully_3();
            // boraTest.ChangeOwnerUnsuccessfully_4();
            // boraTest.LeaveBoardUnsuccessfully_2();
            // boraTest.InvalidDeleteBoard_2();
            // boraTest.LeaveBoardUnsuccessfully_3();

            boraTest.ValidDeleteBoard();
            userService.DeleteAllData();
            boardService.DeleteAllData();





            // boardService.CreateBoard(boardName, email1);
            // boardService.DeleteBoard(boardName, email1);
            // //userTests.LoadUsersTest();
            // userService.CreateUser(email1, password);
            // grading.Register(email1, password);
            // userService.Login(email1, password);
            // grading.Login(email1, password);
            // boardService.CreateBoard(boardName, email1);
            // grading.AddBoard(email1, boardName);
            // boardService.AddTask(email1, boardName, title, description, dueDate);
            // grading.UpdateTaskTitle(email1, boardName, 0, 1, "Hello");


            // BoardTest boraTest = new BoardTest(boardService);
            // boraTest.ValidCreateBoardTest();
            // boraTest.InvalidCreateBoardTest();
            // boraTest.InvalidCreateBoardTest2();
            // boraTest.AddValidTaskTest();
            // boraTest.AddInvalidTaskTest2();
            // boraTest.ValidNextStateTest();
            // boraTest.InvalidNextStateTest();
            // boraTest.InvalidNextStateTest2();
            // boraTest.InvalidNextStateTest3();
            // boraTest.ValidGetColum();
            // boraTest.InvalidGetColum();
            // boraTest.ValidGetColumnLimit();
            // boraTest.InvalidGetColumnLimit();
            // boraTest.ValidGetColumnName();
            // boraTest.InvalidGetColumnName();
            // boraTest.InvalidInProgress();
            // boraTest.ValidLimitColumn();
            // boraTest.InvalidLimitColumn();
            // boraTest.ValidDeleteBoardTest();
            // boraTest.InvalidDeleteBoardTest();
            //
            // Console.WriteLine(grading.DeleteData());
            // Console.WriteLine(grading.Register(email1, password));
            // Console.WriteLine(grading.Logout(email1));
            // //Console.WriteLine(grading.Login("JOHNDOE@gmail.com", password));
            // Console.WriteLine(grading.LoadData());
            // Console.WriteLine(grading.Login(email1, password));
            // Console.WriteLine(grading.Register(email1, password));
            // Console.WriteLine(grading.Logout(email1));
            // //Console.WriteLine(grading.Login("JOHNDOE@gmail.com", password));
            // Console.WriteLine(grading.Login(email1, password));
            // Console.WriteLine(grading.AddBoard(email1, boardName));
            // Console.WriteLine(grading.AddTask(email1, boardName, title, description, dueDate));
            // Console.WriteLine(grading.AssignTask(email1, boardName, 0, 0, email1));
            // Console.WriteLine(grading.AdvanceTask(email1, boardName, 0, 0));
            // // Should fail
            // Console.WriteLine(grading.AddTask(email1, boardName, "", description, dueDate));
            // // New user
            // Console.WriteLine(grading.Register(email2, password2));
            // Console.WriteLine(grading.Login(email2, password2));
            // Console.WriteLine(grading.JoinBoard(email2, 0));
            // Console.WriteLine(grading.AddTask(email2, boardName, ",al;", description, dueDate));
            // Console.WriteLine("Should fail");
            // Console.WriteLine(grading.UpdateTaskTitle(email1, boardName, 0, 1, ""));
            // Console.WriteLine(grading.AssignTask(email1, boardName, 0, 1, email2));
            // Console.WriteLine(grading.AdvanceTask(email2, boardName, 0, 1));
            // Console.WriteLine(grading.GetColumn(email2, boardName, 1));
            // Console.WriteLine(grading.UpdateTaskDueDate(email1, boardName, 1, 1, DateTime.MaxValue));
            // // New user
            // Console.WriteLine(grading.Register(email3, password3));
            // Console.WriteLine(grading.JoinBoard(email3, 0));
            // Console.WriteLine();
            // Console.WriteLine(grading.DeleteData());
            // grading.AddTask(email1, boardName, title, description, dueDate);
            TaskTests tests = new TaskTests(taskService, userService, boardService, grading);
            

            









            //GradingService gradingService = new GradingService();
            //string email = "rrr@gmial.com";
            //string board = "one";
            //gradingService.Register(email, "Aka123k123");
            //gradingService.Login(email, "Aka123k123");
            //string invalid = "jgiosejiooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooojjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjoooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo";
            //Console.WriteLine(gradingService.AddBoard(email, "one"));
            //Console.WriteLine(gradingService.AddTask(email, "one", "bRAND", "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.AddTask(email, "one", "new", "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 0, 0));
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 0, 1));
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 1, 0));
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 1, 1));
            //Console.WriteLine(gradingService.AddTask(email, "one", "new", "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 2, 0));
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 0, 0)); // no such task in column 0
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 0, 2));
            //Console.WriteLine(gradingService.InProgressTasks(email));
            //Console.WriteLine(gradingService.LimitColumn(email, board, 1, 5));
            //Console.WriteLine(gradingService.LimitColumn(email, board, 1, 4));
            //Console.WriteLine(gradingService.LimitColumn(email, board, 1, 10));
            //Console.WriteLine(gradingService.GetColumnLimit(email, board, 1));
            //Console.WriteLine(gradingService.GetColumnName(email, board, 5)); // INVALID NUMBER
            //Console.WriteLine(gradingService.AddTask(email, "three", "new", "HELLOW WORLD", DateTime.Now)); // no such board three
            //Console.WriteLine(gradingService.UpdateTaskDueDate(email, "one", 1, 0, DateTime.Now));// not good , changes to task that not in true coloumn number
            //Console.WriteLine(gradingService.UpdateTaskDueDate(email, "one", 9, 2, DateTime.Now));// not good , changes to invalid coloumn number
            //Console.WriteLine(gradingService.RemoveBoard(email, "one"));
            //Console.WriteLine(gradingService.AddTask(email, "one", "new", "HELLOW WORLD", DateTime.Now));

            //GradingService gradingService = new GradingService();
            //string email = "tamar@gmail.com";
            //string board = "one";
            //Console.WriteLine(gradingService.Register(email, "Aka123k123"));
            //Console.WriteLine(gradingService.Login(email, "Aka123k123"));
            //string invalid = "jgiosejiooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooojjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjoooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo";
            //Console.WriteLine(gradingService.Register(email, "Aka123k123"));
            //Console.WriteLine(gradingService.Login(email, "Aka123k123"));
            //Console.WriteLine(gradingService.Register("klk@klmlk.co.uk", "EEew12221!"));
            //Console.WriteLine(gradingService.Register("klk@asd.com", ""));
            //Console.WriteLine(gradingService.Register("kl#", "EEew12221!"));
            //Console.WriteLine(gradingService.Register("klk@asd.com", "EEew12221!"));
            //Console.WriteLine(gradingService.Login("klk@asd.co1", "EEew12221!"));
            //Console.WriteLine(gradingService.Register("klk", "EEew12221!"));
            //Console.WriteLine(gradingService.Register("klk", "EEew12221!"));

            //Console.WriteLine(gradingService.AddBoard(email, "one"));
            //Console.WriteLine(gradingService.AddTask(email, "one", "bRAND", "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.AddTask(email, "one", "new", "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 0, 0));
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 0, 1));
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 1, 0));
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 1, 1));
            //Console.WriteLine(gradingService.AddTask(email, "one", "new", "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 2, 0));
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 0, 0)); // no such task in column 0
            //Console.WriteLine(gradingService.AdvanceTask(email, "one", 0, 2));
            //Console.WriteLine(gradingService.InProgressTasks(email));
            //Console.WriteLine(gradingService.LimitColumn(email, board, 1, 5));
            //Console.WriteLine(gradingService.LimitColumn(email, board, 1, 4));
            //Console.WriteLine(gradingService.LimitColumn(email, board, 1, 10));
            //Console.WriteLine(gradingService.GetColumnLimit(email, board, 1));
            //Console.WriteLine(gradingService.GetColumnName(email, board, 5)); // INVALID NUMBER
            //Console.WriteLine(gradingService.AddTask(email, "three", "new", "HELLOW WORLD", DateTime.Now)); // no such board three
            //Console.WriteLine(gradingService.UpdateTaskDueDate(email, "one", 1, 0, DateTime.Now));// not good , changes to task that not in true coloumn number
            //Console.WriteLine(gradingService.UpdateTaskDueDate(email, "one", 9, 2, DateTime.Now));// not good , changes to invalid coloumn number
            //Console.WriteLine(gradingService.RemoveBoard(email, "one"));
            //Console.WriteLine(gradingService.AddTask(email, "one", "new", "HELLOW WORLD", DateTime.Now));

            //Console.WriteLine(gradingService.AddBoard(email, "two"));
            //Console.WriteLine(gradingService.AddTask(email, "two", "new", "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.UpdateTaskDueDate(email, "two", 0, 0, DateTime.Now));//
            //Console.WriteLine(gradingService.UpdateTaskTitle(email, "two", 0, 0, "new title"));//
            //Console.WriteLine(gradingService.UpdateTaskTitle(email, "two", 0, 1, "new title"));//no such task
            //Console.WriteLine(gradingService.AdvanceTask(email, "two", 0, 0));
            //Console.WriteLine(gradingService.UpdateTaskTitle(email, "two", 1, 0, "new title"));//
            //Console.WriteLine(gradingService.UpdateTaskTitle(email, "two", 1, 0, invalid));//invalid title
            //Console.WriteLine(gradingService.UpdateTaskDescription(email, "two", 1, 0, "new descp"));
            //Console.WriteLine(gradingService.UpdateTaskDescription(email, "two", 1, 0, invalid));//
            //Console.WriteLine(gradingService.LimitColumn(email, "two", 1, 1));
            //Console.WriteLine(gradingService.AddTask(email, "two", "new task", "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.AdvanceTask(email, "two", 0, 1)); // the column in full




            //BoardTest boraTest = new BoardTest(boardService);
            //boraTest.ValidCreateBoardTest();
            //boraTest.InvalidCreateBoardTest();
            //boraTest.InvalidCreateBoardTest2();
            //boraTest.AddValidTaskTest();
            //boraTest.AddInvalidTaskTest2();
            //boraTest.ValidNextStateTest();
            //boraTest.InvalidNextStateTest();
            //boraTest.InvalidNextStateTest2();
            //boraTest.InvalidNextStateTest3();
            //boraTest.ValidDeleteBoardTest();
            //boraTest.InvalidDeleteBoardTest();
            //boraTest.ValidGetColum();
            //boraTest.InvalidGetColum();
            //boraTest.ValidGetColumnLimit();
            //boraTest.InvalidGetColumnLimit();
            //boraTest.ValidGetColumnName();
            //boraTest.InvalidGetColumnName();
            //boraTest.ValidInProgress();
            //boraTest.InvalidInProgress();
            //boraTest.ValidLimitColumn();
            //boraTest.InvalidLimitColumn();
            //BoardTest boraTest = new BoardTest(boardService);
            //boraTest.AddInvalidTaskTest();
            //UserTests userTests = new UserTests(userController, userService);
            //userTests.createUserTest();
            //userTests.validUserLoginTest();
            //userTests.invalidUserLoginTest();
            //userTests.invalidUserCreation();
            //userTests.invalidLoginTest_2();
            //userTests.invalidUserCreation_2();
            //userTests.logoutTest();
            //userTests.invalidLogoutTest();
            // Console.WriteLine("hey again");
            // GradingService gradingService = new GradingService();
            // string emailll = "whyy@gmail.com";
            // string board1 = "first";
            // gradingService.Register(emailll, "Aka123k123");
            // gradingService.Login(emailll, "Aka123k123");
            // string invalid1 = "jgiosejiooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooojjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjoooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo";
            // Console.WriteLine(gradingService.AddBoard(emailll, board1));
            // Console.WriteLine(gradingService.AddBoard("sds", "ads"));//suppose throw user error
            // Console.WriteLine(gradingService.AddTask(emailll, board1, "bRAND", "HELLOW WORLD", DateTime.Now));
            // Console.WriteLine(gradingService.AddTask(emailll, board1, "new", "HELLOW WORLD", DateTime.Now));
            // Console.WriteLine(gradingService.AddTask("ddd", board1, "new", "HELLOW WORLD", DateTime.Now));//suppose throw user error
            // Console.WriteLine(gradingService.AddBoard(emailll, board1)); //supose to throw board exception
            // Console.WriteLine("NOW I AM CHECKING ADVANCE TASK!!");
            // Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 0, 0));
            // Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 1, 0));
            // Console.WriteLine(gradingService.AdvanceTask("fff", board1, 0, 0));//suppose throw user error
            // Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 2, 0));// at Done
            // Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 0, 1));
            // Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 0, 1));//dose not exist
            //// Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 1, 1));
            // Console.WriteLine(gradingService.AddTask(emailll, board1, "new", "HELLOW WORLD", DateTime.Now)); // no such task in column 0
            // Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 0, 2));
            // Console.WriteLine("NOW I AM InProgress TASK!!");
            // Console.WriteLine(gradingService.InProgressTasks(emailll));
            // Console.WriteLine(gradingService.InProgressTasks("sss"));
            // Console.WriteLine("NOW I AM LimitColumn TASK!!");
            // Console.WriteLine(gradingService.LimitColumn(emailll, board1, 1, 5));//valid
            // Console.WriteLine(gradingService.LimitColumn(emailll, board1, 1, 4));//Valid
            // Console.WriteLine(gradingService.LimitColumn(emailll, board1, 1, -1));//Valid
            // Console.WriteLine(gradingService.LimitColumn(emailll, board1, 2, 0));//InValid amount
            // Console.WriteLine(gradingService.LimitColumn(emailll, board1, 4, 10));// invalid column
            // Console.WriteLine(gradingService.LimitColumn(emailll, "ddd", 2, 10));// invalid board
            // Console.WriteLine(gradingService.LimitColumn("ssds", board1, 1, 10));//user does not exist
            // Console.WriteLine("NOW I AM GetColumnLimit TASK!!");
            // Console.WriteLine(gradingService.GetColumnLimit("emailll", board1, 1));
            // Console.WriteLine(gradingService.GetColumnLimit(emailll, board1, 4));
            // Console.WriteLine(gradingService.GetColumnLimit(emailll, "fff", 1));
            // Console.WriteLine(gradingService.GetColumnLimit(emailll, board1, 1));
            // Console.WriteLine("NOW I AM GetColumnName TASK!!");
            // Console.WriteLine(gradingService.GetColumnName(emailll, board1, 5)); // INVALID NUMBER
            // Console.WriteLine(gradingService.GetColumnName("ds", board1, 2)); // INVALID mail
            // Console.WriteLine(gradingService.GetColumnName(emailll, "njjj", 2)); // INVALID board
            // Console.WriteLine(gradingService.GetColumnName(emailll, board1, 2)); // VALID
            // Console.WriteLine("NOW I AM RemoveBoard TASK!!");
            // Console.WriteLine(gradingService.RemoveBoard("ff", board1));// INVALID mail
            // Console.WriteLine(gradingService.RemoveBoard(emailll, "fff"));// INVALID board
            // Console.WriteLine(gradingService.RemoveBoard(emailll, board1));// VALID 
            // Console.WriteLine("that it for now !!!!");
            // userTests.createUserTest();

            // userTests.validUserLoginTest();
            // userTests.invalidUserLoginTest();
            // userTests.invalidUserCreation();
            // userTests.invalidLoginTest_2();
            // userTests.invalidUserCreation_2();
            // userTests.logoutTest();
            // userTests.invalidLogoutTest();

            Console.WriteLine("bye");

            //BoardTest boraTest = new BoardTest(boardService);
            //boraTest.AddInvalidTaskTest();
            //UserTests userTests = new UserTests(userController, userService);


            // Console.WriteLine("hey again");

            //GradingService gradingService = new GradingService();
            //string emailll = "whyy@gmail.com";
            //string board1 = "first";
            //gradingService.Register(emailll, "Aka123k123");
            //gradingService.Login(emailll, "Aka123k123");
            //Console.WriteLine(gradingService.AddBoard(board1, board1));
            //Console.WriteLine(gradingService.AddTask(emailll, board1, "bRAND", "HELLOW WORLD", DateTime.Now));
            //boraTest.InvalidNextStateTest3();
            //string invalid1 = "jgiosejiooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooojjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjoooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo";
            //Console.WriteLine(gradingService.AddBoard(emailll, board1));
            //Console.WriteLine(gradingService.AddBoard(null, board1));
            //Console.WriteLine("NOW email null TASK!!");
            //Console.WriteLine(gradingService.AddBoard(emailll, null));
            //Console.WriteLine("NOW boardName null TASK!!");
            //Console.WriteLine(gradingService.AddBoard("sds", "ads"));//suppose throw user error
            //Console.WriteLine(gradingService.AddTask(emailll, board1, "bRAND", "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.AddTask(emailll, board1, "new", "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.AddTask("ddd", board1, "new", "HELLOW WORLD", DateTime.Now));//suppose throw user error
            //Console.WriteLine(gradingService.AddBoard(emailll, board1)); //supose to throw board exception
            //Console.WriteLine("from NOW all  null TASK!!");

            //Console.WriteLine(gradingService.AddTask(null, board1, "bRAND", "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.AddTask(emailll, null, "bRAND", "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.AddTask(emailll, board1, null, "HELLOW WORLD", DateTime.Now));
            //Console.WriteLine(gradingService.AddTask(emailll, board1, "bRAND", null, DateTime.Now));

            //Console.WriteLine("NOW I AM CHECKING ADVANCE TASK!!");
            //Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 0, 0));
            //Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 1, 0));
            //Console.WriteLine(gradingService.AdvanceTask("fff", board1, 0, 0));//suppose throw user error
            //Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 2, 0));// at Done
            //Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 0, 1));
            //Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 0, 1));//dose not exist
            //Console.WriteLine("from NOW all  null TASK!!");

            //Console.WriteLine(gradingService.AdvanceTask(null, board1, 0, 0));//null
            //Console.WriteLine(gradingService.AdvanceTask(emailll, null, 0, 0));
            //Console.WriteLine("end all  null TASK!!");

            //Console.WriteLine(gradingService.AddTask(emailll, board1, "new", "HELLOW WORLD", DateTime.Now)); // no such task in column 0
            //Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 0, 2));
            //Console.WriteLine("NOW I AM InProgress TASK!!");

            //Console.WriteLine(gradingService.InProgressTasks(null));

            //Console.WriteLine(gradingService.InProgressTasks(emailll));
            //Console.WriteLine(gradingService.InProgressTasks("sss"));
            //Console.WriteLine("NOW I AM LimitColumn TASK!!");
            //Console.WriteLine(gradingService.LimitColumn(emailll, board1, 1, 5));//valid
            //Console.WriteLine(gradingService.LimitColumn(emailll, board1, 1, 4));//Valid
            //Console.WriteLine(gradingService.LimitColumn(emailll, board1, 1, -1));//Valid
            //Console.WriteLine(gradingService.LimitColumn(emailll, board1, 2, 0));//InValid amount
            //Console.WriteLine(gradingService.LimitColumn(emailll, board1, 4, 10));// invalid column
            //Console.WriteLine(gradingService.LimitColumn(emailll, "ddd", 2, 10));// invalid board
            //Console.WriteLine(gradingService.LimitColumn(emailll, null, 2, 10));// invalid board
            //Console.WriteLine(gradingService.LimitColumn("ssds", board1, 1, 10));//user does not exist
            ////Nullable<int> i = null;

            //Console.WriteLine(gradingService.LimitColumn(emailll, board1, 1, 4));//Valid

            //Console.WriteLine("NOW I AM GetColumnLimit TASK!!");
            //Console.WriteLine(gradingService.GetColumnLimit("emailll", board1, 1));
            //Console.WriteLine(gradingService.GetColumnLimit(emailll, board1, 4));
            //Console.WriteLine(gradingService.GetColumnLimit(emailll, "fff", 1));
            //Console.WriteLine(gradingService.GetColumnLimit(emailll, board1, 1));
            //Console.WriteLine("NOW I AM GetColumnName TASK!!");
            //Console.WriteLine(gradingService.GetColumnName(emailll, board1, 5)); // INVALID NUMBER
            //Console.WriteLine(gradingService.GetColumnName("ds", board1, 2)); // INVALID mail
            //Console.WriteLine(gradingService.GetColumnName(emailll, "njjj", 2)); // INVALID board
            //Console.WriteLine(gradingService.GetColumnName(emailll, board1, 2)); // VALID
            //Console.WriteLine("NOW I AM RemoveBoard TASK!!");
            //Console.WriteLine(gradingService.RemoveBoard("ff", board1));// INVALID mail
            //Console.WriteLine(gradingService.RemoveBoard(emailll, "fff"));// INVALID board
            //Console.WriteLine(gradingService.RemoveBoard(emailll, board1));// VALID 
            //Console.WriteLine("that it for now !!!!");


            // GradingService gradingService = new GradingService();
            // string emailll = "whyy@gmail.com";
            // string board1 = "first";
            // gradingService.Register(emailll, "Aka123k123");
            // gradingService.Login(emailll, "Aka123k123");
            // string invalid1 = "jgiosejiooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooojjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjoooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo";
            // Console.WriteLine(gradingService.AddBoard(emailll, board1));
            // Console.WriteLine(gradingService.AddBoard(null, board1));
            // Console.WriteLine("NOW email null TASK!!");
            // Console.WriteLine(gradingService.AddBoard(emailll, null));
            // Console.WriteLine("NOW boardName null TASK!!");
            // Console.WriteLine(gradingService.AddBoard("sds", "ads"));//suppose throw user error
            // Console.WriteLine(gradingService.AddTask(emailll, board1, "bRAND", "HELLOW WORLD", DateTime.Now));
            // Console.WriteLine(gradingService.AddTask(emailll, board1, "new", "HELLOW WORLD", DateTime.Now));
            // Console.WriteLine(gradingService.AddTask("ddd", board1, "new", "HELLOW WORLD", DateTime.Now));//suppose throw user error
            // Console.WriteLine(gradingService.AddBoard(emailll, board1)); //supose to throw board exception
            // Console.WriteLine("from NOW all  null TASK!!");
            //
            // Console.WriteLine(gradingService.AddTask(null, board1, "bRAND", "HELLOW WORLD", DateTime.Now));
            // Console.WriteLine(gradingService.AddTask(emailll, null, "bRAND", "HELLOW WORLD", DateTime.Now));
            // Console.WriteLine(gradingService.AddTask(emailll, board1, null, "HELLOW WORLD", DateTime.Now));
            // Console.WriteLine(gradingService.AddTask(emailll, board1, "bRAND", null, DateTime.Now));
            //
            // Console.WriteLine("NOW I AM CHECKING ADVANCE TASK!!");
            // Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 0, 0));
            // Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 1, 0));
            // Console.WriteLine(gradingService.AdvanceTask("fff", board1, 0, 0));//suppose throw user error
            // Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 2, 0));// at Done
            // Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 0, 1));
            // Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 0, 1));//dose not exist
            // Console.WriteLine("from NOW all  null TASK!!");
            //
            // Console.WriteLine(gradingService.AdvanceTask(null, board1, 0, 0));//null
            // Console.WriteLine(gradingService.AdvanceTask(emailll, null, 0, 0));
            // Console.WriteLine("end all  null TASK!!");
            //
            // Console.WriteLine(gradingService.AddTask(emailll, board1, "new", "HELLOW WORLD", DateTime.Now)); // no such task in column 0
            // Console.WriteLine(gradingService.AdvanceTask(emailll, board1, 0, 2));
            // Console.WriteLine("NOW I AM InProgress TASK!!");
            //
            // Console.WriteLine(gradingService.InProgressTasks(null));
            //
            // Console.WriteLine(gradingService.InProgressTasks(emailll));
            // Console.WriteLine(gradingService.InProgressTasks("sss"));
            // Console.WriteLine("NOW I AM LimitColumn TASK!!");
            // Console.WriteLine(gradingService.LimitColumn(emailll, board1, 1, 5));//valid
            // Console.WriteLine(gradingService.LimitColumn(emailll, board1, 1, 4));//Valid
            // Console.WriteLine(gradingService.LimitColumn(emailll, board1, 1, -1));//Valid
            // Console.WriteLine(gradingService.LimitColumn(emailll, board1, 2, 0));//InValid amount
            // Console.WriteLine(gradingService.LimitColumn(emailll, board1, 4, 10));// invalid column
            // Console.WriteLine(gradingService.LimitColumn(emailll, "ddd", 2, 10));// invalid board
            // Console.WriteLine(gradingService.LimitColumn(emailll, null, 2, 10));// invalid board
            // Console.WriteLine(gradingService.LimitColumn("ssds", board1, 1, 10));//user does not exist
            //Nullable<int> i = null;
            //
            // Console.WriteLine(gradingService.LimitColumn(emailll, board1, 1, 4));//Valid
            // //
            // Console.WriteLine("NOW I AM GetColumnLimit TASK!!");
            // Console.WriteLine(gradingService.GetColumnLimit("emailll", board1, 1));
            // Console.WriteLine(gradingService.GetColumnLimit(emailll, board1, 4));
            // Console.WriteLine(gradingService.GetColumnLimit(emailll, "fff", 1));
            // Console.WriteLine(gradingService.GetColumnLimit(emailll, board1, 1));
            // Console.WriteLine("NOW I AM GetColumnName TASK!!");
            // Console.WriteLine(gradingService.GetColumnName(emailll, board1, 5)); // INVALID NUMBER
            // Console.WriteLine(gradingService.GetColumnName("ds", board1, 2)); // INVALID mail
            // Console.WriteLine(gradingService.GetColumnName(emailll, "njjj", 2)); // INVALID board
            // Console.WriteLine(gradingService.GetColumnName(emailll, board1, 2)); // VALID
            // Console.WriteLine("NOW I AM RemoveBoard TASK!!");
            // Console.WriteLine(gradingService.RemoveBoard("ff", board1));// INVALID mail
            // Console.WriteLine(gradingService.RemoveBoard(emailll, "fff"));// INVALID board
            // Console.WriteLine(gradingService.RemoveBoard(emailll, board1));// VALID 
            // Console.WriteLine("that it for now !!!!");



            // To delete all existing data from the database -
            // call userService.DeleteAllData() and boardService.DeleteAllData()

            userService.DeleteAllData();
            boardService.DeleteAllData(); // If these calls take a lot of time - the DB might be locked
            Console.WriteLine(" ");
            Console.WriteLine("bye bye!");



        }

    }

}

