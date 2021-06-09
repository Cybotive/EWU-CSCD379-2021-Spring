using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class UserRepository : IUserRepository
    {
        //private SecretSantaContext Context = new SecretSantaContext();

        public User Create(User item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            using SecretSantaContext context = new SecretSantaContext(); // Should've fixed tracking bug

            /*User? existingUser = context.Users
                .Where(
                    user => (user.FirstName.Equals(item.FirstName) && user.LastName.Equals(item.LastName)) || user.Id == item.Id)
                .FirstOrDefault();
            
            if (existingUser is not null)
            {
                return existingUser;
            }*/
            var trackerUpdate = context.Users.Update(item);
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                trackerUpdate.State = EntityState.Unchanged;

                var trackerSave = context.Users.Add(item);
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    trackerSave.State = EntityState.Unchanged;
                }
            }

            return item;
        }

        public User? GetItem(int id)
        {
            using SecretSantaContext context = new SecretSantaContext();
            return context.Users.Find(id);
        }

        public ICollection<User> List()
        {
            using SecretSantaContext context = new SecretSantaContext();
            return context.Users.AsNoTrackingWithIdentityResolution().ToList();
        }

        public bool Remove(int id)
        {
            using SecretSantaContext context = new SecretSantaContext();
            User userToRemove = context.Users.Find(id);

            if (userToRemove is not null)
            {
                var tracker = context.Users.Remove(userToRemove);
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    tracker.State = EntityState.Unchanged;
                    return false;
                }
                
                return true;
            }
            
            return false;
        }

        public void Save(User item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            using SecretSantaContext context = new SecretSantaContext();

            context.Users.Update(item);
            context.SaveChanges();
        }
    }
}
