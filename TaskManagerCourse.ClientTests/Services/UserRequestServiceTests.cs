using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Services.Tests
{
    [TestClass()]
    public class UserRequestServiceTests
    {
        [TestMethod()]
        public void GetTokenTest()
        {
            var token = new UserRequestService().GetToken("admin", "qwerty123");
            Console.WriteLine(token.access_token);
            Assert.IsNotNull(token);
        }
        [TestMethod()]
        public void CreateUserTest()
        {
            var service = new UserRequestService();

            var token = service.GetToken("admin", "qwerty123");

            UserModel userTest = new UserModel("Boba", "Biba", "BibaBoba@ya.ru", "12345", UserStatus.Editor, "8800553535");
            var result = service.CreateUser(token, userTest);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }
        [TestMethod()]
        public void GetAllUsersTest()
        {
            var service = new UserRequestService();

            var token = service.GetToken("admin", "qwerty123");

            var result = service.GetAllUsers(token);
            Console.WriteLine(result.Count());
            Assert.AreNotEqual(Array.Empty<UserModel>, result.ToArray());
        }
        [TestMethod()]
        public void DeleteUserTest()
        {
            var service = new UserRequestService();

            var token = service.GetToken("admin", "qwerty123");

            var result = service.DeleteUser(token, 13);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
        [TestMethod()]
        public void AddMultipleUsersTest()
        {
            var service = new UserRequestService();

            var token = service.GetToken("admin", "qwerty123");

            UserModel userTest = new UserModel("Boba", "Biba", "BibaBoba@ya.ru", "12345", UserStatus.Editor, "8800553535");
            UserModel userTest1 = new UserModel("Pupa", "Lupa", "PupaLupa@ya.ru", "12345", UserStatus.User, "8900656565");
            UserModel userTest2 = new UserModel("Bobba", "Bobka", "BoobaBoobka@ya.ru", "12345", UserStatus.User, "88005537575");
            List<UserModel> users = new List<UserModel>() { userTest, userTest1, userTest2};
            
            var result = service.CreateMultipleUsers(token, users);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }
        [TestMethod()]
        public void UpadteeUserTest()
        {
            var service = new UserRequestService();

            var token = service.GetToken("admin", "qwerty123");
            UserModel userTest = new UserModel("ABobba", "ABobka", "BoobaBoobka@ya.ru", "12345", UserStatus.Editor, "+78005537575");
            userTest.Id = 18;
            var result = service.UpdateUser(token, userTest);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}