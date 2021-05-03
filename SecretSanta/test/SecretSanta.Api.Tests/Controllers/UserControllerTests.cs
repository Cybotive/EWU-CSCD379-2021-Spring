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
using SecretSanta.Api.Controllers;
using SecretSanta.Api.Dto;

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
            UsersController controllerTemp = new(null!);

            //Assert - Handled in tags
        }
        
        [TestMethod]
        public async Task Put_WithValidData_UpdatesEvent()
        {
            //Arrange
            var factory = new CustomWebApplicationFactory();
            HttpClient client = factory.CreateClient();
            UpdateUser updateUser = new()
            {
                FirstName = "Firstname",
                LastName = "Lastname"
            };

            //Act
            HttpResponseMessage response = await client.PutAsJsonAsync("/api/users/1", updateUser);

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
