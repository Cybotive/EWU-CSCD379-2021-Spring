using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class UserRepository : IUserRepository
    {
        private SecretSantaContext Context = new SecretSantaContext();

        public User Create(User item)
        {
            Context = new SecretSantaContext(); // Should've fixed tracking bug
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }


            var tracker = Context.Users.Add(item);
            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                tracker.State = EntityState.Unchanged;
            }
            return item;
        }

        public User? GetItem(int id)
        {
            return Context.Users.Find(id);
        }

        public ICollection<User> List()
        {
            return Context.Users.AsNoTracking().ToList();
        }

        public bool Remove(int id)
        {
            User userToRemove = Context.Users.Find(id);

            if (userToRemove is not null)
            {
                var tracker = Context.Users.Remove(userToRemove);
                try
                {
                    Context.SaveChanges();
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

            Context.Users.Update(item);
            Context.SaveChanges();
        }
    }
}
