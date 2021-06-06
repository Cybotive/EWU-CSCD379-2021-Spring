using System;

namespace SecretSanta.Data
{
    public class Gift
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; } = "";
        public string? Url { get; set; } = "";
        public int Priority { get; set; }
        public User Receiver { get; set; }

        public Gift(User receiver)
        {
            Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        }

        public Gift()
        {
            if(Receiver is null)
                throw new ArgumentNullException(nameof(Receiver));
        }
    }
}
