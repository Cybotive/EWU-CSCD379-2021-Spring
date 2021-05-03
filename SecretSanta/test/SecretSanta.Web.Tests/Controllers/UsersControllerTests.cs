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
            User user1 = new() { Id = 1, FirstName = "Place0", LastName = "Holder0" };
            User user2 = new() { Id = 2, FirstName = "Place1", LastName = "Holder1" };
            TestableUsersClient usersClient = Factory.Client;
            usersClient.GetAllUsersReturnValue = new List<User>()
            {
                user1, user2
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
            HttpClient client = Factory.CreateClient();
            TestableUsersClient usersClient = Factory.Client;

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
            Assert.AreEqual(1, usersClient.PostAsyncInvocationCount);
            Assert.AreEqual(1, usersClient.PostAsyncInvokedParameters.Count);
            Assert.AreEqual(_TestFirstName, usersClient.PostAsyncInvokedParameters[0].FirstName);
            Assert.AreEqual(_TestLastName, usersClient.PostAsyncInvokedParameters[0].LastName);
        }
    }
}
