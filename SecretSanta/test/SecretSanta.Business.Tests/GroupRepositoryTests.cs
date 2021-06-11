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
                Id = 42,
                Name = "ThisIsATestOf..."
            };

            sut.Remove(group.Id);

            Group createdGroup = sut.Create(group);

            Group? retrievedGroup = sut.GetItem(createdGroup.Id);
            Assert.AreEqual(group.Id, retrievedGroup?.Id);
            Assert.AreEqual(group.Name, retrievedGroup?.Name);
            Assert.IsNotNull(retrievedGroup?.Users);
            Assert.IsTrue(group.Users.SequenceEqual(retrievedGroup?.Users!));
            Assert.IsNotNull(retrievedGroup?.Assignments);
            Assert.IsTrue(group.Assignments.SequenceEqual(retrievedGroup?.Assignments!));

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
                Id = 42,
                Name = "Group",
            });

            Group? group = sut.GetItem(42);

            Assert.AreEqual(42, group?.Id);
            Assert.AreEqual("Group", group!.Name);
        }

        [TestMethod]
        public void List_WithGroups_ReturnsPopulatedGroupList()
        {
            GroupRepository sut = new();
            sut.Create(new()
            {
                Id = 62,
                Name = "Group",
            });
            sut.Create(new()
            {
                Id = 620,
                Name = "Group",
            });

            List<Group> groups = sut.List().ToList();

            Assert.IsTrue(groups.Count >= 2);
            Assert.IsNotNull(groups[0]);
            Assert.IsNotNull(groups[1]);
        }

        [TestMethod]
        [DataRow(-1, false)]
        [DataRow(42, true)]
        public void Remove_WithId_ReturnsExpected(int id, bool expected)
        {
            GroupRepository sut = new();
            sut.Create(new()
            {
                Id = 42,
                Name = "Group"
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
            sut.Create(new()
            {
                Id = 42,
                Name = "Group"
            });

            AssignmentResult result = sut.GenerateAssignments(42);

            Assert.AreEqual($"Group Group must have at least three users", result.ErrorMessage);
        }

        [TestMethod]
        public void GenerateAssignments_WithValidGroup_CreatesAssignments()
        {
            GroupRepository sut = new();
            Group? group = sut.Create(new()
            {
                //Id = 42,
                Name = "Group"
            });
            
            //group.Users.Clear();
            //sut.Save(group);

            group.Users.Add(new User { FirstName = "John", LastName = "Doe" });
            group.Users.Add(new User { FirstName = "Jane", LastName = "Smith" });
            group.Users.Add(new User { FirstName = "Bob", LastName = "Jones" });

            sut.Save(group);

            group = sut.GetItem(group.Id);
            Assert.IsNotNull(group);

            AssignmentResult result = sut.GenerateAssignments(group.Id);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(3, group.Assignments.Count);
            Assert.AreEqual(3, group.Assignments.Select(x => x.Giver.FirstName).Distinct().Count());
            Assert.AreEqual(3, group.Assignments.Select(x => x.Receiver.FirstName).Distinct().Count());
            Assert.IsFalse(group.Assignments.Any(x => x.Giver.FirstName == x.Receiver.FirstName));
        }
    }
}
