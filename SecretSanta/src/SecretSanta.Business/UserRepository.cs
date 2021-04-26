using System.Linq;
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
            return DataDeleteMe.Users.FirstOrDefault(x => x.Id == id);
        }

        public ICollection<User> List()
        {
            return DataDeleteMe.Users;
        }

        public bool Remove(int id)
        {
            User? toDelete = DataDeleteMe.Users.FirstOrDefault(x => x.Id == id);

            if(toDelete is not null){
                return DataDeleteMe.Users.Remove(toDelete);
            }

            return false;
        }

        public void Save(User item)
        {
            Remove(item.Id);
            Create(item);
        }
    }
}