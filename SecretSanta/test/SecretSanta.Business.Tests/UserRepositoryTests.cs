using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SecretSanta.Business;
using SecretSanta.Data;

// Naming Reference: <MethodBeingTested>_<ConditionBeingTested>_<ExpectedOutcome>()

namespace SecretSanta.Api.Controllers
{
    [TestClass]
    public class UserRepositoryTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DataDeleteMe.Users.Clear();
        }

        [TestMethod]
        public void GetItem_ValidId_ReturnsUserWithSameId()
        {
            //Arrange
            UserRepository userRepository = new();
            User expectedUser = new User() { Id=491, FirstName="Unit", LastName="Test" };
            DataDeleteMe.Users.Add(expectedUser);

            //Act
            User gottenUser = userRepository.GetItem(expectedUser.Id);

            //Assert
            Assert.AreEqual(expectedUser.Id, gottenUser.Id);
        }
    }
}