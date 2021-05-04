namespace SecretSanta.Api.Tests.Business
{
    using System.Collections.Generic;
    using SecretSanta.Business;
    using SecretSanta.Data;

    public class TestableUserRepository : IUserRepository
    {
        public User? CreatedUser { get; set; } = null;
        public User Create(User item)
        {
            CreatedUser = item;
            return CreatedUser;
        }

        public User? ItemUser { get; set; } //Set this beforehand in order to test functionality
        public int ItemId { get; set; }
        public User? GetItem(int id)
        {
            ItemId = id;
            return ItemUser;
        }

        public List<User> UserList { get; set; } = new(); //Set this beforehand in order to test functionality
        public ICollection<User> List()
        {
            return UserList;
        }

        public User? UserToRemove { get; set; } = new(); //Set this beforehand in order to test functionality
        public bool DeleteResult { get; set; } = false;
        public bool Remove(int id)
        {
            User? userBefore = UserToRemove;

            if(userBefore is null || UserToRemove is null || id != UserToRemove.Id)
            {
                return false;
            }

            UserToRemove = null; //"Remove" user

            DeleteResult = ! userBefore.Equals(UserToRemove);
            return DeleteResult;
        }

        public User? SavedUser {get; set;}
        public void Save(User item)
        {
            SavedUser = item;
        }
    }
}