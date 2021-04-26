using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SecretSanta.Business;
using SecretSanta.Data;

// Naming Reference: <MethodBeingTested>_<ConditionBeingTested>_<ExpectedOutcome>()

namespace SecretSanta.Api.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private static UsersController controller;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            controller = new(new UserRepository());
        }

        [TestMethod]
        public void Get_WithData_ReturnsUsers()
        {
            //Arrange - Handled in ClassInitialize

            //Act
            IEnumerable<User> users = controller.Get();

            //Assert
            Assert.IsTrue(users.Any());
        }

        [TestMethod]
        public void Get_ValidIndex_ReturnsUserWithMatchingId()
        {
            //Arrange
            int id = 1;

            //Act
            User returnedUser = controller.Get(id);

            //Assert
            Assert.AreEqual(id, returnedUser.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Get_NegativeIndex_ThrowsException()
        {
            //Arrange
            int id = -1;

            //Act
            User returnedUser = controller.Get(id);

            //Assert - Handled in tags
        }
    }
}