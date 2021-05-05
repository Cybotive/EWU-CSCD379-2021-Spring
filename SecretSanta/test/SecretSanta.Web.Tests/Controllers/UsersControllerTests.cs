using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SecretSanta.Web.Api;
using SecretSanta.Web.ViewModels;
using SecretSanta.Web.Tests.Api;
using System.Collections.Generic;
using System;
using System.Net;

namespace SecretSanta.Web.Tests
{
    [TestClass]
    public class UsersControllerTests
    {
        private CustomWebApplicationFactory Factory { get; } = new();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UsersController_WithNullParameter_ThrowsException()
        {
            //Arrange - Nothing to arrange

            //Act
            Web.Controllers.UsersController controllerTemp = new(null!);

            //Assert - Handled in tags
        }

        [TestMethod]
        public void UsersController_WithValidParameter_DoesNotThrowException()
        {
            //Arrange - Nothing to arrange

            //Act
            Web.Controllers.UsersController controllerTemp = new(new UsersClient(Factory.CreateClient()));

            //Assert - Nothing to assert
        }

        [TestMethod]
        public async Task Index_WithUsers_InvokesGetAllAsync()
        {
            //Arrange
            FullUser user1 = new() { Id = 1, FirstName = "Place0", LastName = "Holder0" };
            FullUser user2 = new() { Id = 2, FirstName = "Place1", LastName = "Holder1" };
            TestableUsersClient usersClient = Factory.Client;
            usersClient.GetAllUsersReturnValue = new List<FullUser>()
            {
                user1, user2
            };
            int startingInvocation = usersClient.GetAllAsyncInvocationCount;

            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.GetAsync("/Users");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(startingInvocation + 1, usersClient.GetAllAsyncInvocationCount);
        }

        [TestMethod]
        public async Task Index_WithoutUsers_InvokesGetAllAsync()
        {
            //Arrange
            TestableUsersClient usersClient = Factory.Client;
            usersClient.GetAllUsersReturnValue = new List<FullUser>();
            int startingInvocation = usersClient.GetAllAsyncInvocationCount;

            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.GetAsync("/Users");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(startingInvocation + 1, usersClient.GetAllAsyncInvocationCount);
        }

        [TestMethod]
        public async Task Create_WithValidModel_InvokesPostAsync()
        {
            //Arrange
            HttpClient client = Factory.CreateClient();
            TestableUsersClient usersClient = Factory.Client;
            int startingInvocation = usersClient.PostAsyncInvocationCount;

            string _TestFirstName = "CWVM";
            string _TestLastName = "IPA";
            
            Dictionary<string, string?> values = new()
            {
                { nameof(UserViewModel.FirstName), _TestFirstName },
                { nameof(UserViewModel.LastName), _TestLastName }
            };
            FormUrlEncodedContent content = new(values!);

            //Act
            HttpResponseMessage response = await client.PostAsync("/Users/Create", content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(startingInvocation + 1, usersClient.PostAsyncInvocationCount);
            Assert.AreEqual(1, usersClient.PostAsyncInvokedParameters.Count);
        }

        [TestMethod]
        public async Task Create_WithInvalidModel_InvokesPostAsync() // Since UserViewModel properties are all nullable
        {
            //Arrange
            HttpClient client = Factory.CreateClient();
            TestableUsersClient usersClient = Factory.Client;
            int startingInvocation = usersClient.PostAsyncInvocationCount;
            
            Dictionary<string, string?> values = new()
            {
                { nameof(UserViewModel.Id), null },
                { nameof(UserViewModel.LastName), null },
            };
            FormUrlEncodedContent content = new(values!);

            //Act
            HttpResponseMessage response = await client.PostAsync("/Users/Create", content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(startingInvocation + 1, usersClient.PostAsyncInvocationCount);
        }

        [TestMethod]
        public async Task Edit_WithValidId_InvokesGetAsync()
        {
            //Arrange
            FullUser user = new() { Id = 909, FirstName = "Place0", LastName = "Holder0" };
            TestableUsersClient usersClient = Factory.Client;
            usersClient.GetAsyncFullUser = user;
            int startingInvocation = usersClient.GetAsyncInvocationCount;

            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.GetAsync("/Users/Edit/" + user.Id);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(startingInvocation + 1, usersClient.GetAsyncInvocationCount);
        }

        [TestMethod]
        public async Task Edit_WithInvalidId_InvokesGetAsync()
        {
            //Arrange
            FullUser user = new() { Id = 909, FirstName = "Place0", LastName = "Holder0" };
            TestableUsersClient usersClient = Factory.Client;
            usersClient.GetAsyncFullUser = user;
            int startingInvocation = usersClient.GetAsyncInvocationCount;

            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.GetAsync("/Users/Edit/" + (user.Id - 1));

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
            Assert.AreEqual(startingInvocation + 1, usersClient.GetAsyncInvocationCount);
        }

        [TestMethod]
        public async Task Edit_WithValidModel_InvokesPutAsync()
        {
            //Arrange
            FullUser user = new() { Id = 342, FirstName = "EWVM", LastName = "IPuA" };
            TestableUsersClient usersClient = Factory.Client;
            usersClient.FullUserToUpdate = user;
            int startingInvocation = usersClient.PutAsyncInvocationCount;

            HttpClient client = Factory.CreateClient();
            
            string _UpdatedFirstName = "EwvmUpdated";
            string _UpdatedLastName = "IpuaUpdated";

            Dictionary<string, string?> values = new()
            {
                { nameof(UserViewModel.FirstName), _UpdatedFirstName },
                { nameof(UserViewModel.LastName), _UpdatedLastName }
            };
            FormUrlEncodedContent content = new(values!);

            //Act
            HttpResponseMessage response = await client.PostAsync("/Users/Edit/" + user.Id, content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(startingInvocation + 1, usersClient.PutAsyncInvocationCount);
        }

        [TestMethod]
        public async Task Edit_WithInvalidModel_InvokesPutAsync() // Since UserViewModel properties are all nullable
        {
            //Arrange
            FullUser user = new() { Id = 342, FirstName = "EWVM", LastName = "IPuA" };
            TestableUsersClient usersClient = Factory.Client;
            usersClient.FullUserToUpdate = user;
            int startingInvocation = usersClient.PutAsyncInvocationCount;

            HttpClient client = Factory.CreateClient();

            Dictionary<string, string?> values = new()
            {
                { nameof(UserViewModel.FirstName), null },
            };
            FormUrlEncodedContent content = new(values!);

            //Act
            HttpResponseMessage response = await client.PostAsync("/Users/Edit/" + user.Id, content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(startingInvocation + 1, usersClient.PutAsyncInvocationCount);
        }

        [TestMethod]
        public async Task Delete_WithExistentId_InvokesDeleteAsync()
        {
            //Arrange
            FullUser user = new() { Id = 909, FirstName = "Place0", LastName = "Holder0" };
            TestableUsersClient usersClient = Factory.Client;
            usersClient.DeleteAsyncFullUser = user;
            int startingInvocation = usersClient.DeleteAsyncInvocationCount;

            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.PostAsync("/Users/Delete/" + user.Id, null!);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(startingInvocation + 1, usersClient.DeleteAsyncInvocationCount);
        }

        [TestMethod]
        public async Task Delete_WithNonExistentId_InvokesDeleteAsync()
        {
            //Arrange
            FullUser user = new() { Id = 909, FirstName = "Place0", LastName = "Holder0" };
            TestableUsersClient usersClient = Factory.Client;
            usersClient.DeleteAsyncFullUser = user;
            int startingInvocation = usersClient.DeleteAsyncInvocationCount;

            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.PostAsync("/Users/Delete/" + (user.Id - 1), null!);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(startingInvocation + 1, usersClient.DeleteAsyncInvocationCount);
        }
    }
}
