using System.Collections.Generic;
using SecretSanta.Web.ViewModels;

namespace SecretSanta.Web.Data
{
    public static class MockData
    {
        public static List<UserViewModel> Users = new List<UserViewModel>
        {
            new UserViewModel { Id = 0, FirstName = "Inigo", LastName = "Montoya" },
            new UserViewModel { Id = 1, FirstName = "Princess", LastName = "Buttercup" },
            new UserViewModel { Id = 2, FirstName = "Prince", LastName = "Humperdink" },
            new UserViewModel { Id = 3, FirstName = "Count", LastName = "Rugen" },
            new UserViewModel { Id = 4, FirstName = "Miracle", LastName = "Max" },
        };

        public static List<GroupViewModel> Groups = new List<GroupViewModel>
        {
            new GroupViewModel { Id = 0, Name = "GoonSquad" },
            new GroupViewModel { Id = 1, Name = "Rodents Of Unusual Size" },
        };

        public static List<GiftViewModel> Gifts = new List<GiftViewModel>
        {
            new GiftViewModel { Id = 0, Title = "Drone", Description = "A fun little way to get a birds eye view", Url="https://www.google.com", Priority = 2, UserId = 1 },
            new GiftViewModel { Id = 1, Title = "Rat Poison", Description = "Hopefully a way to get rid of those unusual size rodents", Url="https://www.google.com", Priority = 1, UserId = 1 },
        };
    }
}
