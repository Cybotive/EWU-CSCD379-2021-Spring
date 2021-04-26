using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SecretSanta.Api.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        [TestMethod]
        public void Get_WithData_ReturnsUsers()
        {
            //Arrange
            UsersController controller = new();

            //Act
            IEnumerable<string> users = controller.Get();

            //Assert
            Assert.IsTrue(users.Any());
        }
    }
}