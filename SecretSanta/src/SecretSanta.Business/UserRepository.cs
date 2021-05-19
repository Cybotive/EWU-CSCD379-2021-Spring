using System.Collections.Generic;
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

            int newId = 0;

            while (MockData.Users.ContainsKey(newId))
            {
                newId++;
            }
            
            item.Id = newId;

            MockData.Users[item.Id] = item;
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
            return MockData.Users.Values;
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
