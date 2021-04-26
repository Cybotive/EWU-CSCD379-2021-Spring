using System.Collections.Generic;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class UserRepository : IUserRepository
    {
        public User Create(User item)
        {
            DataDeleteMe.Users.Add(item);
            return item;
        }

        public User? GetItem(int id)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<User> List()
        {
            return DataDeleteMe.Users;
        }

        public bool Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Save(User item)
        {
            throw new System.NotImplementedException();
        }
    }
}