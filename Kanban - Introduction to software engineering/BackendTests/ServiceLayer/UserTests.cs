using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.Buissnes_Layer;

namespace BackendTests.ServiceLayer
{
    [TestClass()]
    public class UserTests
    {
        private UserController userController;
        public UserService userService;

        public UserTests(UserController UC, UserService US)
        {
            this.userController = UC;
            this.userService = US;
        }

        /// <summary>
        /// This method tests a valid creation of a new user in the system according to requirement 7.
        /// </summary>
        [TestMethod()]
        public void createUserTest()
        {
            Console.WriteLine("Create user test- ");
            Response response = new Response(null, null);
            Console.WriteLine(ToJson.toJson(response));
            Assert.AreEqual(userService.CreateUser("johndoe@gmail.com", "Hash123"), ToJson.toJson(response));
            Console.WriteLine("");

        }

        /// <summary>
        /// This method tests a valid login of an existing user in the system according to requirement 8.
        /// </summary>
        [TestMethod()]
        public void validUserLoginTest()
        {
            Console.WriteLine("Successful login test!");
            Response response = new Response(null, "johndoe@gmail.com");
            Console.WriteLine(ToJson.toJson(response));

            Assert.AreEqual(userService.Login("johndoe@gmail.com", "Hash123"), ToJson.toJson(response));
            Console.WriteLine("");
        }

        /// <summary>
        /// This method tests an invalid login of a user due to a wrong password, according to requirement 1.
        /// </summary>
        public void invalidUserLoginTest()
        {
            Console.WriteLine("Invalid login test:");
            Response response = new Response("Wrong password");
            Console.WriteLine(ToJson.toJson(response));
            Assert.AreEqual(userService.Login("johndoe@gmail.com", "wrong_password"), ToJson.toJson(response));
            Console.WriteLine(" ");
        }

        /// <summary>
        /// This method tests an invalid login of a user which doesn't exist, according to requirement 1.
        /// </summary>
        public void invalidLoginTest_2()
        {
            Console.WriteLine("Invalid login test");
            Response response = new Response("User does not exist");
            Console.WriteLine(ToJson.toJson(response));
            Assert.AreEqual(userService.Login("null@gmail.com", "wrong"), ToJson.toJson(response));
            Console.WriteLine(" ");
        }

        /// <summary>
        /// This method tests an invalid user creation - due to a short password(under 6 characters) according to requirement 2.
        /// </summary>
        public void invalidUserCreation()
        {
            Console.WriteLine("Invalid registration test: ");
            Response response =
                new Response(
                    "Illegal password. A legal password must be 6-20 characters" +
                    " and must contain an Upper case, a lower case and a number");
            Console.WriteLine(ToJson.toJson(response));
            Assert.AreEqual(userService.CreateUser("tom@gmail.com", "1234"), ToJson.toJson(response));
            Console.WriteLine(" ");
        }

        /// <summary>
        /// This method tests an invalid user creation - due to an email which already exists - according to requirement 3.
        /// </summary>
        public void invalidUserCreation_2()
        {
            Console.WriteLine("Invalid registration test: ");
            Response response = new Response("User already exists");
            Console.WriteLine(ToJson.toJson(response));
            Assert.AreEqual(userService.CreateUser("johndoe@gmail.com", "Ai9898"), ToJson.toJson(response));
            Console.WriteLine(" ");
        }

        /// <summary>
        /// This method tests an invalid user creation - due to an email which already exists - according to requirement 3.
        /// </summary>
        public void invalidUserCreation_3()
        {
            Console.WriteLine("Invalid registration test: ");
            Response response = new Response("Not a valid email address");
            Console.WriteLine(ToJson.toJson(response));
            Assert.AreEqual(userService.CreateUser("_@gmailcom", "Ai9898"), ToJson.toJson(response));
            Console.WriteLine(" ");
        }


        /// <summary>
        /// This method tests the logout of a user- according to requirement 8.
        /// </summary>
        public void logoutTest()
        {
            Console.WriteLine("Logout test: ");
            Response response = new Response(null);
            Console.WriteLine(ToJson.toJson(response));
            Assert.AreEqual(userService.logout("johndoe@gmail.com"), ToJson.toJson(response));
            Console.WriteLine(" ");
        }

        /// <summary>
        /// This method tests an invalid logout - of a user that is already logged out - according to requirement 8.
        ///</summary>
        public void invalidLogoutTest()
        {
            Console.WriteLine("Invalid logout test: ");
            Response response = new Response("User is already logged out");
            Console.WriteLine(ToJson.toJson(response));
            Assert.AreEqual(userService.logout("johndoe@gmail.com"), ToJson.toJson(response));
            Console.WriteLine(" ");
        }

        public string LoadUsersTest()
        {
            return userService.LoadData();
        }

        public string DeleteData()
        {
            return userService.DeleteAllData();
        }
    }
}
