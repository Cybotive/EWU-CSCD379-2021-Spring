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
        public async Task Put_WithValidData_UpdatesUser()
        {
            //Arrange
            var factory = new CustomWebApplicationFactory();
            TestableUserRepository testableRepo = factory.UserRepo;
            HttpClient client = factory.CreateClient();

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
        public async Task List_WithData_ReturnsUserList()
        {
            
        }
    }
}
