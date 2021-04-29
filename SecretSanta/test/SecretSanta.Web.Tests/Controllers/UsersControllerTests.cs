using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using SecretSanta.Web.Api;
using SecretSanta.Web.Tests.Api;

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

            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.GetAsync("/Users");

            //Assert
            response.EnsureSuccessStatusCode();

        }
    }
}
