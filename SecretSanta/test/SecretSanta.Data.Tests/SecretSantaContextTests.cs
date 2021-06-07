using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class SecretSantaContextTests
    {
        [TestMethod]
        public void UsersDbSet_AddingValidUser_IncrementsCount()
        {
            //Arrange
            using SecretSantaContext db = new SecretSantaContext();
            User user = new User() { FirstName = "FirstNameTest", LastName = "LastNameTest" };

            try // Prep
            {
                User? userToRemove = db.Users.Where(u => u.FirstName == user.FirstName && u.LastName == user.LastName).FirstOrDefault();
                if (userToRemove is not null)
                {
                    db.Users.Remove(userToRemove);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException)
            {
                // Don't want to fail test because of this
            }

            int countBefore = db.Users.Count();

            //Act
            User userAdded = db.Users.Add(user).Entity;
            db.SaveChanges();

            //Assert
            Assert.AreEqual(countBefore + 1, db.Users.Count());

            try // Clean-up
            {
                db.Users.Remove(userAdded);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                // Don't want to fail test because of this
            }
        }

        [TestMethod]
        public void UsersDbSet_WithData_CanRetrieveUsers()
        {
            //Arrange
            using SecretSantaContext db = new SecretSantaContext();
            User user = new User() { FirstName = "FirstNameTest", LastName = "LastNameTest" };

            try // Prep
            {
                User? userToRemove = db.Users.Where(u => u.FirstName == user.FirstName && u.LastName == user.LastName).FirstOrDefault();
                if (userToRemove is not null)
                {
                    db.Users.Remove(userToRemove);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException)
            {
                // Don't want to fail test because of this
            }

            int countBefore = db.Users.Count();

            //Act
            User userAdded = db.Users.Add(user).Entity;
            db.SaveChanges();

            List<User> list = db.Users.ToList();

            //Assert
            Assert.AreEqual(countBefore + 1, db.Users.Count());

            try // Clean-up
            {
                db.Users.Remove(userAdded);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                // Don't want to fail test because of this
            }
        }
    }
}
