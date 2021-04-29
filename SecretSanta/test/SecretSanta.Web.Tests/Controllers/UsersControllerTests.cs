using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SecretSanta.Web.Api;
using SecretSanta.Web.ViewModels;
using SecretSanta.Web.Tests.Api;
using System.Collections.Generic;

namespace SecretSanta.Web.Tests
{
    [TestClass]
    public class UsersControllerTests
    {
    private CustomWebApplicationFactory Factory { get; } = new();

        [TestMethod]
        public async Task Index_WithEvents_InvokesGetAllAsync()
        {
            //Arrange
            User user0 = new() { Id = 0, FirstName = "Place0", LastName = "Holder0" };
            User user1 = new() { Id = 1, FirstName = "Place1", LastName = "Holder1" };
            TestableUsersClient usersClient = Factory.Client;
            usersClient.GetAllUsersReturnValue = new List<User>()
            {
                user0, user1
            };

            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.GetAsync("/Users/");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(1, usersClient.GetAllAsyncInvocationCount);
        }

        [TestMethod]
        public async Task Create_WithValidModel_InvokesPostAsync()
        {
            //Arrange
            TestableUsersClient usersClient = Factory.Client;
            HttpClient client = Factory.CreateClient();
            UserViewModel user = new() { FirstName = "User0First", LastName = "User0Last" };
            
            Dictionary<string, string?> values = new()
            {
                {"FirstName", "Place"},
                {"LastName", "Holder"}
            };
            FormUrlEncodedContent content = new(values!);

            //Act
            //HttpResponseMessage response = await client.PostAsJsonAsync("/Users/Create", user);
            HttpResponseMessage response = await client.PostAsync("/Users/Create", content);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(1, usersClient.PostAsyncInvocationCount);
            Assert.AreEqual(1, usersClient.PostAsyncInvokedParameters.Count);
            Assert.AreEqual(user.FirstName, usersClient.PostAsyncInvokedParameters[0].FirstName);
            Assert.AreEqual(user.LastName, usersClient.PostAsyncInvokedParameters[0].LastName);
        }
    }
}
