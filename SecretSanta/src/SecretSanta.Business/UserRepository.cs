using System;
using System.Collections.Generic;
using System.Linq;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class UserRepository : IUserRepository
    {
        private static readonly SecretSantaContext Context = new SecretSantaContext();

        /*public User Create(User item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            MockData.Users[item.Id] = item;
            return item;
        }*/
        public User Create(User item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            Context.Users.Add(item);
            Context.SaveChanges();
            return item;
        }

        public User? GetItem(int id)
        {
            if (MockData.Users.TryGetValue(id, out User? user))
            {
                return user;
            }
            return null;
        }

        public ICollection<User> List()
        {
            List<User> list = Context.Users.ToList();
            return list; // Temp var for debugging
        }

        public bool Remove(int id)
        {
            return MockData.Users.Remove(id);
        }

        public void Save(User item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            MockData.Users[item.Id] = item;
        }
    }
}
