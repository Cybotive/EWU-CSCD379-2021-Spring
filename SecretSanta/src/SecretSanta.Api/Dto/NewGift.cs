namespace SecretSanta.Api.Dto
{
    public class NewGift
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public int? Priority { get; set; }
        public User? Receiver { get; set; }
    }
}
