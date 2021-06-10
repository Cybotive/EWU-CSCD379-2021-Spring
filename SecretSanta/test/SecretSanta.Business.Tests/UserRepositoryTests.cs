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
                Id = 42
            };

            User createdUser = sut.Create(user);

            User? retrievedUser = sut.GetItem(createdUser.Id);
            Assert.AreEqual(user, retrievedUser);
        }

        [TestMethod]
        public void Create_WithOrWithoutItem_CanAddItem()
        {
            UserRepository sut = new();
            User user = new()
            {
                Id = 42
            };

            sut.Remove(user.Id);
            
            User createdUser = sut.Create(user);
            User createdUserDuplicate = sut.Create(user);

            Assert.AreEqual(user, createdUser);
            Assert.AreEqual(user, createdUserDuplicate);

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
            sut.Create(new() 
            { 
                Id = 52,
                FirstName = "First",
                LastName = "Last"
            });

            User? user = sut.GetItem(42);

            Assert.AreEqual(42, user?.Id);
            Assert.AreEqual("First", user!.FirstName);
            Assert.AreEqual("Last", user.LastName);
        }

        [TestMethod]
        public void List_WithUsers_ReturnsPopulatedUserList()
        {
            UserRepository sut = new();
            sut.Create(new()
            {
                Id = 62,
                FirstName = "First",
                LastName = "Last"
            });
            sut.Create(new()
            {
                Id = 620,
                FirstName = "First",
                LastName = "Last"
            });

            List<User> users = sut.List().ToList();

            Assert.IsTrue(users.Count >= 2);
            Assert.IsNotNull(users[0]);
            Assert.IsNotNull(users[1]);
        }

        [TestMethod]
        [DataRow(-1, false)]
        [DataRow(42, true)]
        public void Remove_WithInvalidId_ReturnsTrue(int id, bool expected)
        {
            UserRepository sut = new();
            sut.Create(new()
            {
                Id = 42,
                FirstName = "First",
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
