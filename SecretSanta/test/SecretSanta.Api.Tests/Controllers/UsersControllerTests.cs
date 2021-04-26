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
        [ExpectedException(typeof(ArgumentNullException))]
        public void UsersController_NullParameter_ThrowsException()
        {
            //Arrange - Nothing to arrange

            //Act
            UsersController controllerTemp = new(null);

            //Assert - Handled in tags
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
        public void Get_ValidIndex_ReturnsUser()
        {
            //Arrange
            int _Index = 1;

            //Act
            User returnedUser = controller.Get(_Index);

            //Assert
            Assert.IsNotNull(returnedUser);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Get_NegativeIndex_ThrowsException()
        {
            //Arrange
            int _Id = -1;

            //Act
            User returnedUser = controller.Get(_Id);

            //Assert - Handled in tags
        }

        [TestMethod]
        public void Delete_WithData_RemovesOneUser()
        {
            //Arrange
            int _Index = 0;
            int startingCount = controller.Get().Count();

            //Act
            controller.Delete(_Index);
            IEnumerable<User> users = controller.Get();

            //Assert
            Assert.AreEqual(startingCount - 1, users.Count());
        }

        [TestMethod]
        public void Post_ValidUser_AddsOneUser()
        {
            //Arrange
            int startingCount = controller.Get().Count();
            User user = new User() { Id=243, FirstName="Place", LastName="Holder" };

            //Act
            controller.Post(user);

            //Assert
            Assert.AreEqual(startingCount + 1, controller.Get().Count());
        }

        [TestMethod]
        public void Put_WithData_UpdatesEntryAtIndex()
        {
            //Arrange
            int _Index = 0;
            User expectedUser = new User() { Id=432, FirstName="Place", LastName="Holder" };

            //Act
            controller.Put(_Index, expectedUser);
            User returnedUser = controller.Get(_Index);

            //Assert
            Assert.AreEqual(expectedUser.Id, returnedUser.Id);
        }
    }
}