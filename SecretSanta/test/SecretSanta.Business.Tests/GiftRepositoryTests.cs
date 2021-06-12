using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftRepositoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullItem_ThrowsArgumentException()
        {
            GiftRepository sut = new();

            sut.Create(null!);
        }

        [TestMethod]
        public void Create_WithItem_CanGetItem()
        {
            GiftRepository sut = new();
            Gift user = new()
            {
                //Id = 42, // Unnecessary, but reduces database bloat
                Title = "ThisIsATestOf...",
                Receiver = new()
            };

            sut.Remove(user.Id);

            Gift createdGift = sut.Create(user);

            Gift? retrievedGift = sut.GetItem(createdGift.Id);

            Assert.IsNotNull(retrievedGift);
            Assert.AreEqual(user.Id, retrievedGift.Id);
            Assert.AreEqual(user.Title, retrievedGift.Title);

            sut.Remove(user.Id);
        }

        [TestMethod]
        public void Create_WithDuplicateId_ReturnsExistingGift()
        {
            //Arrange
            GiftRepository sut = new();

            //Act
            Gift user = new()
            {
                //Id = 42,
                Title = "Create_WithDuplicateId_ReturnsExistingGift",
                Receiver = new()
            };
            Gift createdGift = sut.Create(user);

            Gift userNew = new()
            {
                Id = createdGift.Id,
                Title = "Create_WithDuplicateId_ReturnsExistingGift_REPLACEYOURNAME",
                Receiver = new()
            };
            Gift createdGiftOverwrite = sut.Create(userNew);
            
            Gift? retrievedGift = sut.GetItem(userNew.Id);

            //Assert
            Assert.AreEqual(user.Id, createdGift.Id);
            Assert.AreEqual(user.Title, createdGift.Title);
            Assert.AreEqual(createdGift.Id, createdGiftOverwrite.Id);
            Assert.AreEqual(createdGift.Title, createdGiftOverwrite.Title);

            Assert.IsNotNull(retrievedGift);
            // Ensure name wasn't "Updated/Saved" through Create()
            Assert.AreEqual(user.Title, retrievedGift.Title);

            sut.Remove(user.Id);
        }

        [TestMethod]
        public void GetItem_WithBadId_ReturnsNull()
        {
            GiftRepository sut = new();

            Gift? user = sut.GetItem(-1);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetItem_WithValidId_ReturnsGift()
        {
            GiftRepository sut = new();
            
            Gift createdGift = sut.Create(new() 
            { 
                Id = 42,
                Title = "GetItem_WithValidId_ReturnsGift",
                Receiver = new()
            });
            Gift createdDynamicGift = sut.Create(new() 
            { 
                //Id = 42,
                Title = "GetItem_WithValidId_ReturnsGift_Dynamic",
                Receiver = new()
            });

            Gift? retrievedGift = sut.GetItem(createdGift.Id);
            Gift? retrievedDynamicGift = sut.GetItem(createdDynamicGift.Id);

            Assert.IsNotNull(retrievedGift);
            Assert.IsNotNull(retrievedDynamicGift);

            Assert.AreEqual(createdGift.Id, retrievedGift.Id);
            Assert.AreEqual(createdDynamicGift.Id, retrievedDynamicGift.Id);

            sut.Remove(createdGift.Id);
            sut.Remove(createdDynamicGift.Id);
        }

        [TestMethod]
        public void List_WithGifts_ReturnsAccurateCount()
        {
            GiftRepository sut = new();

            int countBefore = sut.List().Count;

            Gift userFirst = sut.Create(new()
            {
                Title = "List_WithGifts_ReturnsAccurateCount_A",
                Receiver = new()
            });
            Gift userSecond = sut.Create(new()
            {
                Title = "List_WithGifts_ReturnsAccurateCount_B",
                Receiver = new()
            });

            List<Gift> users = sut.List().ToList();

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
            GiftRepository sut = new();

            sut.Remove(42);

            sut.Create(new()
            {
                Id = 42,
                Title = "Remove_WithId_ReturnsExpected",
                Receiver = new()
            });

            Assert.AreEqual(expected, sut.Remove(id));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_NullItem_ThrowsArgumentException()
        {
            GiftRepository sut = new();

            sut.Save(null!);
        }

        [TestMethod]
        public void Save_WithValidItem_SavesItem()
        {
            //Arrange
            GiftRepository sut = new();

            sut.Remove(42);

            Gift initialGift = new Gift() { Id = 42, Title = "BeforeUpdate", Receiver = new() };
            //Gift updatedGift = new Gift() { Id = 42, Title = "AfterUpdate", Receiver = initialGift.Receiver };
            sut.Create(initialGift);

            sut = new();

            initialGift.Title = "AfterUpdate";
            //Act
            sut.Save(initialGift);

            //Assert
            Gift? gotGift = sut.GetItem(42);
            Assert.IsNotNull(gotGift);
            Assert.AreEqual(42, gotGift.Id);
            Assert.AreEqual(initialGift.Title, gotGift.Title);
            
            sut.Remove(42);
        }
    }
}
