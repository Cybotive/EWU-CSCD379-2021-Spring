using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GroupRepositoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullItem_ThrowsArgumentException()
        {
            GroupRepository sut = new();

            sut.Create(null!);
        }

        [TestMethod]
        public void Create_WithItem_CanGetItem()
        {
            GroupRepository sut = new();
            Group group = new()
            {
                //Id = 42, // Unnecessary, but reduces database bloat
                Name = "ThisIsATestOf...",
            };

            sut.Remove(group.Id);

            Group createdGroup = sut.Create(group);

            Group? retrievedGroup = sut.GetItem(createdGroup.Id);

            Assert.IsNotNull(retrievedGroup);
            Assert.AreEqual(group.Id, retrievedGroup.Id);
            Assert.AreEqual(group.Name, retrievedGroup.Name);

            sut.Remove(group.Id);
        }

        [TestMethod]
        public void Create_WithDuplicateId_ReturnsExistingGroup()
        {
            //Arrange
            GroupRepository sut = new();

            //Act
            Group group = new()
            {
                //Id = 42,
                Name = "Create_WithDuplicateId_ReturnsExistingGroup"
            };
            Group createdGroup = sut.Create(group);

            Group groupNew = new()
            {
                Id = createdGroup.Id,
                Name = "Create_WithDuplicateId_ReturnsExistingGroup_REPLACEYOURNAME"
            };
            Group createdGroupOverwrite = sut.Create(groupNew);
            
            Group? retrievedGroup = sut.GetItem(groupNew.Id);

            //Assert
            Assert.AreEqual(group.Id, createdGroup.Id);
            Assert.AreEqual(group.Name, createdGroup.Name);
            Assert.AreEqual(createdGroup.Id, createdGroupOverwrite.Id);
            Assert.AreEqual(createdGroup.Name, createdGroupOverwrite.Name);

            Assert.IsNotNull(retrievedGroup);
            // Ensure name wasn't "Updated/Saved" through Create()
            Assert.AreEqual(group.Name, retrievedGroup.Name);

            sut.Remove(group.Id);
        }

        [TestMethod]
        public void Create_WithItem_RelatesAllMembers()
        {
            GroupRepository sut = new();
            User testUserReceiver = new User() { FirstName = "testUserReceiver", LastName = "testUserReceiver" };
            User testUser = new User() { FirstName = "testUser", LastName = "testUser", Gifts = { new(), new(), new() }};
            Group group = new()
            {
                Name = "Create_WithItem_RelatesAllMembers",
                Users = new() { testUser, testUserReceiver }
            };

            sut.Remove(group.Id);

            Group createdGroup = sut.Create(group);

            Group? retrievedGroup = sut.GetItem(createdGroup.Id);

            Assert.AreEqual(group.Name, retrievedGroup?.Name);

            Assert.IsNotNull(retrievedGroup?.Users);
            Assert.AreEqual(group.Users.Count, retrievedGroup.Users.Count);

            User userLocal = group.Users.Where(user => user.Id == testUser.Id).Single();
            User userRetrieved = retrievedGroup.Users.Where(user => user.Id == testUser.Id).Single();

            Assert.AreEqual(userLocal.Id, userRetrieved.Id);
            Assert.AreEqual(userLocal.FirstName, userRetrieved.FirstName);
            Assert.AreEqual(userLocal.LastName, userRetrieved.LastName);

            Assert.AreEqual(userLocal.Gifts.Count, userRetrieved.Gifts.Count);
            Assert.AreEqual(userLocal.Gifts.First().Receiver.Id, userRetrieved.Gifts.First().Receiver.Id);

            Assert.IsNotNull(retrievedGroup?.Assignments);
            Assert.AreEqual(group.Assignments.Count, retrievedGroup.Assignments.Count);

            sut.Remove(group.Id);
        }

        [TestMethod]
        public void GetItem_WithBadId_ReturnsNull()
        {
            GroupRepository sut = new();

            Group? group = sut.GetItem(-1);

            Assert.IsNull(group);
        }

        [TestMethod]
        public void GetItem_WithValidId_ReturnsGroup()
        {
            GroupRepository sut = new();
            
            sut.Create(new() 
            { 
                Id = 42
            });

            Group? group = sut.GetItem(42);

            Assert.AreEqual(42, group?.Id);
        }

        [TestMethod]
        public void List_WithGroups_ReturnsPopulatedGroupList()
        {
            GroupRepository sut = new();

            int countBefore = sut.List().Count;

            Group groupFirst = sut.Create(new()
            {
                Name = "List_WithGroups_ReturnsPopulatedGroupList_A",
            });
            Group groupSecond = sut.Create(new()
            {
                Name = "List_WithGroups_ReturnsPopulatedGroupList_B",
            });

            List<Group> groups = sut.List().ToList();

            Assert.IsTrue(groups.Count >= 2);
            Assert.AreEqual(countBefore + 2, groups.Count);

            sut.Remove(groupFirst.Id);
            sut.Remove(groupSecond.Id);
        }

        [TestMethod]
        [DataRow(-1, false)]
        [DataRow(42, true)]
        public void Remove_WithId_ReturnsExpected(int id, bool expected)
        {
            GroupRepository sut = new();

            sut.Remove(42);

            sut.Create(new()
            {
                Id = 42,
                Name = "Remove_WithId_ReturnsExpected"
            });

            Assert.AreEqual(expected, sut.Remove(id));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_NullItem_ThrowsArgumentException()
        {
            GroupRepository sut = new();

            sut.Save(null!);
        }

        [TestMethod]
        public void Save_WithValidItem_SavesItem()
        {
            //Arrange
            GroupRepository sut = new();

            sut.Remove(42);

            Group initialGroup = new Group() { Id = 42, Name = "BeforeUpdate" };
            Group updatedGroup = new Group() { Id = 42, Name = "AfterUpdate" };
            sut.Create(initialGroup);

            //Act
            sut.Save(updatedGroup);

            //Assert
            Group? gotGroup = sut.GetItem(42);
            Assert.IsNotNull(gotGroup);
            Assert.AreEqual(42, gotGroup.Id);
            Assert.AreEqual(updatedGroup.Name, gotGroup.Name);
            
            sut.Remove(42);
        }

        [TestMethod]
        public void GenerateAssignments_WithInvalidId_ReturnsError()
        {
            GroupRepository sut = new();

            AssignmentResult result = sut.GenerateAssignments(-2);

            Assert.AreEqual("Group not found", result.ErrorMessage);
        }

        [TestMethod]
        public void GenerateAssignments_WithLessThanThreeUsers_ReturnsError()
        {
            GroupRepository sut = new();
            Group createdGroup = sut.Create(new()
            {
                //Id = 42,
                Name = "GenerateAssignments_WithLessThanThreeUsers_ReturnsError"
            });

            AssignmentResult result = sut.GenerateAssignments(createdGroup.Id);

            Assert.AreEqual($"Group {createdGroup.Name} must have at least three users", result.ErrorMessage);
        }

        [TestMethod]
        public void GenerateAssignments_WithValidGroup_CreatesAssignments()
        {
            GroupRepository sut = new();
            Group? group = sut.Create(new()
            {
                //Id = 42,
                Name = "GenerateAssignments_WithValidGroup_CreatesAssignments"
            });

            group.Users.Add(new User { FirstName = "John", LastName = "Doe" });
            group.Users.Add(new User { FirstName = "Jane", LastName = "Smith" });
            group.Users.Add(new User { FirstName = "Bob", LastName = "Jones" });

            sut.Save(group);

            group = sut.GetItem(group.Id);
            Assert.IsNotNull(group);

            AssignmentResult result = sut.GenerateAssignments(group.Id);

            var sanityCheckDoesNotThrowException = sut.List();

            group = sut.GetItem(group.Id);
            Assert.IsNotNull(group);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(3, group.Assignments.Count);
            Assert.AreEqual(3, group.Assignments.Select(x => x.Giver.FirstName).Distinct().Count());
            Assert.AreEqual(3, group.Assignments.Select(x => x.Receiver.FirstName).Distinct().Count());
            Assert.IsFalse(group.Assignments.Any(x => x.Giver.FirstName == x.Receiver.FirstName));

            sut.Remove(group.Id);
        }
    }
}
