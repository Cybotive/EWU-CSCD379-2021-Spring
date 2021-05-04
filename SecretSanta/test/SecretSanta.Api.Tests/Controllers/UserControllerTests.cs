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
        public void UsersController_NullParameter_ThrowsException()
        {
            //Arrange - Nothing to arrange

            //Act
            Api.Controllers.UsersController controllerTemp = new(null!);

            //Assert - Handled in tags
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
        public async Task Put_WithValidData_UpdatesUser()
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

        private static bool AreUsersEqual(User userA, User userB){
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
