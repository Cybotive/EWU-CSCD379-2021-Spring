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

        public ICollection<User> List()
        {
            throw new System.NotImplementedException();
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