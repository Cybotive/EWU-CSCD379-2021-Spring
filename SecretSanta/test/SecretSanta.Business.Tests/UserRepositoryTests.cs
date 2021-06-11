using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullItem_ThrowsArgumentException()
        {
            UserRepository sut = new();

            sut.Create(null!);
        }

        [TestMethod]
        public void Create_WithItem_CanGetItem()
        {
            UserRepository sut = new();
            User user = new()
            {
                //Id = 42, // Unnecessary, but reduces database bloat
                FirstName = "ThisIsATestOf...",
                LastName = "...TheEmergencyBroadcastSystem",
            };

            sut.Remove(user.Id);

            User createdUser = sut.Create(user);

            User? retrievedUser = sut.GetItem(createdUser.Id);

            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(user.Id, retrievedUser.Id);
            Assert.AreEqual(user.FirstName, retrievedUser.FirstName);
            Assert.AreEqual(user.LastName, retrievedUser.LastName);

            sut.Remove(user.Id);
        }

        [TestMethod]
        public void Create_WithDuplicateId_ReturnsExistingUser()
        {
            //Arrange
            UserRepository sut = new();

            //Act
            User user = new()
            {
                //Id = 42,
                FirstName = "Create_WithDuplicateId_ReturnsExistingUser"
            };
            User createdUser = sut.Create(user);

            User userNew = new()
            {
                Id = createdUser.Id,
                FirstName = "Create_WithDuplicateId_ReturnsExistingUser_REPLACEYOURNAME"
            };
            User createdUserOverwrite = sut.Create(userNew);
            
            User? retrievedUser = sut.GetItem(userNew.Id);

            //Assert
            Assert.AreEqual(user.Id, createdUser.Id);
            Assert.AreEqual(user.FirstName, createdUser.FirstName);
            Assert.AreEqual(createdUser.Id, createdUserOverwrite.Id);
            Assert.AreEqual(createdUser.FirstName, createdUserOverwrite.FirstName);

            Assert.IsNotNull(retrievedUser);
            // Ensure name wasn't "Updated/Saved" through Create()
            Assert.AreEqual(user.FirstName, retrievedUser.FirstName);

            sut.Remove(user.Id);
        }

        [TestMethod]
        public void GetItem_WithBadId_ReturnsNull()
        {
            UserRepository sut = new();

            User? user = sut.GetItem(-1);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetItem_WithValidId_ReturnsUser()
        {
            UserRepository sut = new();
            
            User createdUser = sut.Create(new() 
            { 
                Id = 42,
                FirstName = "First",
                LastName = "Last"
            });
            User createdDynamicUser = sut.Create(new() 
            { 
                //Id = 42,
                FirstName = "First",
                LastName = "Last"
            });

            User? retrievedUser = sut.GetItem(createdUser.Id);
            User? retrievedDynamicUser = sut.GetItem(createdDynamicUser.Id);

            Assert.IsNotNull(retrievedUser);
            Assert.IsNotNull(retrievedDynamicUser);

            Assert.AreEqual(createdUser.Id, retrievedUser.Id);
            Assert.AreEqual(createdDynamicUser.Id, retrievedDynamicUser.Id);

            sut.Remove(createdUser.Id);
            sut.Remove(createdDynamicUser.Id);
        }

        [TestMethod]
        public void List_WithUsers_ReturnsAccurateCount()
        {
            UserRepository sut = new();

            int countBefore = sut.List().Count;

            User userFirst = sut.Create(new()
            {
                FirstName = "List_WithUsers_ReturnsAccurateCount_A",
                LastName = "Last"
            });
            User userSecond = sut.Create(new()
            {
                FirstName = "List_WithUsers_ReturnsAccurateCount_B",
                LastName = "Last"
            });

            List<User> users = sut.List().ToList();

            Assert.IsTrue(users.Count >= 2);
            Assert.AreEqual(countBefore + 2, users.Count);
            
            sut.Remove(userFirst.Id);
            sut.Remove(userSecond.Id);
        }

        [TestMethod]
        [DataRow(-1, false)]
        [DataRow(42, true)]
        public void Remove_WithId_ReturnsExpected(int id, bool expected)
        {
            UserRepository sut = new();

            sut.Remove(42);

            sut.Create(new()
            {
                Id = 42,
                FirstName = "Remove_WithId_ReturnsExpected",
                LastName = "Last"
            });

            Assert.AreEqual(expected, sut.Remove(id));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_NullItem_ThrowsArgumentException()
        {
            UserRepository sut = new();

            sut.Save(null!);
        }

        [TestMethod]
        public void Save_WithValidItem_SavesItem()
        {
            //Arrange
            UserRepository sut = new();

            sut.Remove(42);

            User initialUser = new User() { Id = 42, FirstName = "BeforeUpdate" };
            User updatedUser = new User() { Id = 42, FirstName = "AfterUpdate" };
            sut.Create(initialUser);

            //Act
            sut.Save(updatedUser);

            //Assert
            User? gotUser = sut.GetItem(42);
            Assert.IsNotNull(gotUser);
            Assert.AreEqual(42, gotUser.Id);
            Assert.AreEqual(updatedUser.FirstName, gotUser.FirstName);
            
            sut.Remove(42);
        }
    }
}
