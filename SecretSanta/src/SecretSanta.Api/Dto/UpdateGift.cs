namespace SecretSanta.Api.Dto
{
    public class UpdateGift
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public int? Priority { get; set; }
    }
}
