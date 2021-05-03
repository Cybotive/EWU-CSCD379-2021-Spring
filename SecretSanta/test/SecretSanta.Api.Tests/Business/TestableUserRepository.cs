namespace SecretSanta.Api.Tests.Business
{
    using System.Collections.Generic;
    using SecretSanta.Business;
    using SecretSanta.Data;

    public class TestableUserRepository : IUserRepository
    {
        public User Create(User item)
        {
            throw new System.NotImplementedException();
        }

        public User? ItemUser { get; set; }
        public int ItemId { get; set; }
        public User? GetItem(int id)
        {
            ItemId = id;
            return ItemUser;
        }

        public List<User> UserList { get; set; } = new();
        public ICollection<User> List()
        {
            return UserList;
        }

        public bool Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public User? SavedUser {get; set;}
        public void Save(User item)
        {
            SavedUser = item;
        }
    }
}