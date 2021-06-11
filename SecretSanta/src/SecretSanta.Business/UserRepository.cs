using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class UserRepository : IUserRepository
    {
        public User Create(User item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            using (SecretSantaContext context = new SecretSantaContext())
            {
                User user = context.Users.Find(item.Id);
                if (user is not null)
                {
                    return user;
                }
            }

            using (SecretSantaContext context = new SecretSantaContext())
            {
                var settledUser = context.Users.Add(item);
                context.SaveChanges();
                // Likely unnecessary, but ensures Id gets updated from db
                item.Id = settledUser.CurrentValues.GetValue<int>("Id");
            }

            return item;
        }

        public User? GetItem(int id)
        {
            using SecretSantaContext context = new SecretSantaContext();

            return context.Users
                .Include(user => user.Groups)
                .ThenInclude(group => group.Assignments)
                .Include(user => user.Gifts)
                .Where(user => user.Id == id)
                .SingleOrDefault();
        }

        public ICollection<User> List()
        {
            using SecretSantaContext context = new SecretSantaContext();

            return context.Users
                .Include(user => user.Groups)
                .ThenInclude(group => group.Assignments)
                .Include(user => user.Gifts)
                .ToList();
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

            /*using (SecretSantaContext context = new SecretSantaContext())
            {
                if (context.Users.Find(item.Id) is null)
                {
                    return;
                }
            }*/

            using (SecretSantaContext context = new SecretSantaContext())
            {
                context.Users.Update(item);
                context.SaveChanges();
            }
        }
    }
}
