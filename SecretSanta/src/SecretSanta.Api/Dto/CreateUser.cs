namespace SecretSanta.Api.Dto
{
    // Domain Transfer Object
    public class CreateUser
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}