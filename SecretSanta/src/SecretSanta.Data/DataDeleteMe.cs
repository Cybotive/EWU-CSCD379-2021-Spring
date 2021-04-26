using System.Collections.Generic;

namespace SecretSanta.Data
{
    public static class DataDeleteMe
    {
        public static List<User> Users { get; } = new()
        {
            new User() { Id=0, FirstName="Cy", LastName="botive" },
            new User() { Id=1, FirstName="User", LastName="WithId1" },
        };
    }
}