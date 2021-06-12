using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Api.Dto
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<Group>? Groups { get; set; } = new();

        public List<Gift>? Gifts { get; set; } = new();

        public static User? ToDto(Data.User? user)
        {
            if (user is null) return null;
            return new User
            {
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Gifts = user.Gifts.Select(gift => Gift.ToDto(gift)).ToList()!
            };
        }

        public static Data.User? FromDto(User? user)
        {
            if (user is null) return null;
            return new Data.User
            {
                Id = user.Id,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? ""
            };
        }
    }
}
