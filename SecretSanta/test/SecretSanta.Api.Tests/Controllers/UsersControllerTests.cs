using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SecretSanta.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        [TestMethod]
        public void Get_WithData_ReturnsUsers()
        {
            //Arrange
            UsersController controller = new(new UserRepository());

            //Act
            IEnumerable<User> users = controller.Get();

            //Assert
            Assert.IsTrue(users.Any());
        }
    }
}