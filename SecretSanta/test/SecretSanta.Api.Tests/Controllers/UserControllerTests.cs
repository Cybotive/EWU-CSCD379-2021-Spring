using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Dto;
using SecretSanta.Api.Tests.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        private static CustomWebApplicationFactory Factory { get; } = new();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UsersController_WithNullParameter_ThrowsException()
        {
            //Arrange - Nothing to arrange

            //Act
            Api.Controllers.UsersController controllerTemp = new(null!);

            //Assert - Handled in tags
        }

        [TestMethod]
        public void UsersController_WithValidParameter_DoesNotThrowException()
        {
            //Arrange - Nothing to arrange

            //Act
            Api.Controllers.UsersController controllerTemp = new(new TestableUserRepository());

            //Assert - Nothing to assert
        }

        [TestMethod]
        public async Task Get_WithData_ReturnsAccurateUserList()
        {
            //Arrange
            TestableUserRepository testableRepo = Factory.UserRepo;
            HttpClient client = Factory.CreateClient();

            User user0 = new User() { Id = 998, FirstName = "Who", LastName = "Knows" };
            User user1 = new User() { Id = 999, FirstName = "I", LastName = "Do" };

            testableRepo.UserList = new List<User>() { user0, user1, };

            //Act
            HttpResponseMessage response = await client.GetAsync("/api/users");
            List<User>? userList = await response.Content.ReadFromJsonAsync<List<User>?>();
            
            //Assert
            response.EnsureSuccessStatusCode();

            if(userList is null)
            {
                Assert.Fail("No List was received. \'" + nameof(userList) + "\' is null.");
                return;
            }

            Assert.AreEqual(testableRepo.UserList.Count, userList.Count);
            Assert.IsTrue(AreUsersEqual(testableRepo.UserList[0], userList[0]));
            Assert.IsTrue(AreUsersEqual(testableRepo.UserList[1], userList[1]));
        }

        [TestMethod]
        public async Task Get_WithoutData_ReturnsEmptyList()
        {
            //Arrange
            TestableUserRepository testableRepo = Factory.UserRepo;
            HttpClient client = Factory.CreateClient();
            testableRepo.UserList = new List<User>();

            //Act
            HttpResponseMessage response = await client.GetAsync("/api/users");
            List<User>? userList = await response.Content.ReadFromJsonAsync<List<User>?>();
            
            //Assert
            response.EnsureSuccessStatusCode();

            if(userList is null)
            {
                Assert.Fail("No List was received. \'" + nameof(userList) + "\' is null.");
                return;
            }

            Assert.AreEqual(0, userList.Count);
        }

        [TestMethod]
        public async Task Get_UsingIdParameterAndWithData_ReturnsAccurateUser()
        {
            //Arrange
            TestableUserRepository testableRepo = Factory.UserRepo;
            HttpClient client = Factory.CreateClient();

            User user = new User() { Id = 998, FirstName = "Who", LastName = "Knows" };
            testableRepo.ItemUser = user;

            //Act
            HttpResponseMessage response = await client.GetAsync("/api/users/" + user.Id);
            User? userReceived = await response.Content.ReadFromJsonAsync<User?>();
            
            //Assert
            response.EnsureSuccessStatusCode();

            if(user is null)
            {
                Assert.Fail("No item was received. \'" + nameof(user) + "\' is null.");
                return;
            }

            Assert.AreEqual(testableRepo.ItemId, user.Id);
            Assert.IsTrue(AreUsersEqual(testableRepo.ItemUser, user));
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(4242)]
        public async Task Get_WithInvalidId_RespondsWith404(int id)
        {
            //Arrange
            TestableUserRepository testableRepo = Factory.UserRepo;
            HttpClient client = Factory.CreateClient();

            User user = new User() { Id = 998, FirstName = "Who", LastName = "Knows" };
            testableRepo.ItemUser = user;

            //Act
            HttpResponseMessage response = await client.GetAsync("/api/users/" + id);
            User? userReceived = await response.Content.ReadFromJsonAsync<User?>();
            
            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task Delete_WithExistentId_RemovesUserOfId()
        {
            //Arrange
            TestableUserRepository testableRepo = Factory.UserRepo;
            HttpClient client = Factory.CreateClient();

            User user = new User() { Id = 998, FirstName = "Who", LastName = "Knows" };
            testableRepo.UserToRemove = user;

            //Act
            HttpResponseMessage response = await client.DeleteAsync("/api/users/" + user.Id);
            
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(true, testableRepo.DeleteResult);
            Assert.IsNull(testableRepo.UserToRemove);
        }

        [TestMethod]
        public async Task Delete_WithNonExistentId_RespondsWith404()
        {
            //Arrange
            TestableUserRepository testableRepo = Factory.UserRepo;
            HttpClient client = Factory.CreateClient();

            User user = new User() { Id = 998, FirstName = "Who", LastName = "Knows" };
            testableRepo.UserToRemove = user;

            //Act
            HttpResponseMessage response = await client.DeleteAsync("/api/users/" + (user.Id + 1));
            
            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task Post_WithValidUser_AddsUser()
        {
            //Arrange
            TestableUserRepository testableRepo = Factory.UserRepo;
            HttpClient client = Factory.CreateClient();

            User user = new User { Id = 334, FirstName = "IdIs334", LastName = "Truth", };
            testableRepo.CreatedUser = user;

            //Act
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/users", user);
            User? userReflected = await response.Content.ReadFromJsonAsync<User?>();

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.IsTrue(AreUsersEqual(user, userReflected));
        }

        [TestMethod]
        public async Task Post_WithNullUser_RespondsWith400()
        {
            //Arrange
            TestableUserRepository testableRepo = Factory.UserRepo;
            HttpClient client = Factory.CreateClient();

            User? user = null;
            testableRepo.CreatedUser = user;

            //Act
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/users", user);
            User? userReflected = await response.Content.ReadFromJsonAsync<User?>();

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Put_WithValidUser_UpdatesUser()
        {
            //Arrange
            TestableUserRepository testableRepo = Factory.UserRepo;
            HttpClient client = Factory.CreateClient();

            User targetUser = new User
            {
                Id = 456
            };
            testableRepo.ItemUser = targetUser;

            string _TestFirstName = "Firstname";
            string _TestLastName = "Lastname";
            UpdateUser updateUser = new()
            {
                FirstName = _TestFirstName,
                LastName = _TestLastName
            };

            //Act
            HttpResponseMessage response = await client.PutAsJsonAsync("/api/users/" + targetUser.Id, updateUser);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(targetUser.Id, testableRepo.SavedUser?.Id);
            Assert.AreEqual(_TestFirstName, testableRepo.SavedUser?.FirstName);
            Assert.AreEqual(_TestLastName, testableRepo.SavedUser?.LastName);
        }

        [TestMethod]
        public async Task Put_WithNullUser_RespondsWith400()
        {
            //Arrange
            TestableUserRepository testableRepo = Factory.UserRepo;
            HttpClient client = Factory.CreateClient();

            User targetUser = new User
            {
                Id = 456
            };
            testableRepo.ItemUser = targetUser;

            UpdateUser? updateUser = null;

            //Act
            HttpResponseMessage response = await client.PutAsJsonAsync("/api/users/" + targetUser.Id, updateUser);

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Put_WithNonExistentId_RespondsWith404()
        {
            //Arrange
            TestableUserRepository testableRepo = Factory.UserRepo;
            HttpClient client = Factory.CreateClient();

            User targetUser = new User
            {
                Id = 456
            };
            testableRepo.ItemUser = targetUser;

            UpdateUser updateUser = new()
            {
                FirstName = "Firstname",
                LastName = "Lastname"
            };

            //Act
            HttpResponseMessage response = await client.PutAsJsonAsync("/api/users/" + (targetUser.Id + 1), updateUser);

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        private static bool AreUsersEqual(User? userA, User? userB){
            if(userA is null || userB is null)
            {
                if(userA is not null || userB is not null)
                    return false;
                
                return true;
            }

            if(userA.Id != userB.Id)
                return false;

            if(userA.FirstName != userB.FirstName)
                return false;

            if(userA.LastName != userB.LastName)
                return false;

            return true;
        }
    }
}
