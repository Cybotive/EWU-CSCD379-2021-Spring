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
        private static UserRepository userRepo;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DataDeleteMe.Users.Clear();
            userRepo = new UserRepository();
        }

        [TestMethod]
        public void Create_ValidParameter_AddsOneUser(){
            //Arrange
            int startingCount = userRepo.List().Count();
            User user = new User() { Id=243, FirstName="Place", LastName="Holder" };

            //Act
            userRepo.Create(user);

            //Assert
            Assert.AreEqual(startingCount + 1, userRepo.List().Count());
        }

        [TestMethod]
        public void GetItem_ValidId_ReturnsUserWithSameId()
        {
            //Arrange
            User expectedUser = new User() { Id=491, FirstName="Unit", LastName="Test" };
            DataDeleteMe.Users.Add(expectedUser);

            //Act
            User gottenUser = userRepo.GetItem(expectedUser.Id);

            //Assert
            Assert.AreEqual(expectedUser.Id, gottenUser.Id);
        }

        [TestMethod]
        public void List_WithData_ReturnsUsers()
        {
            //Arrange - Handled in ClassInitialize

            //Act
            ICollection<User> users = userRepo.List();

            //Assert
            Assert.IsTrue(users.Any());
        }

        [TestMethod]
        public void Remove_WithData_RemovesMatchingUser()
        {
            //Arrange
            int _Id = 909;
            User expectedUser = new User() { Id=_Id, FirstName="Place", LastName="Holder"};
            userRepo.Create(expectedUser);

            //Act
            User returnedUserBefore = userRepo.GetItem(_Id);
            bool removeSuccessful = userRepo.Remove(_Id);
            User returnedUserAfter = userRepo.GetItem(_Id);

            //Assert
            Assert.IsNotNull(returnedUserBefore);
            Assert.IsTrue(removeSuccessful);
            Assert.IsNull(returnedUserAfter);
        }

        [TestMethod]
        public void Save_WithExistingUser_UpdatesUserInfo()
        {
            //Arrange
            int _Id = 1009;
            User startingUser = new User() { Id=_Id, FirstName="Place", LastName="Holder"};
            userRepo.Create(startingUser);
            User updatedUser = new User() { Id=_Id, FirstName="Updated", LastName="Holder"};

            //Act
            userRepo.Save(updatedUser);

            //Assert
            Assert.AreEqual(userRepo.GetItem(startingUser.Id).FirstName, updatedUser.FirstName);
        }
    }
}