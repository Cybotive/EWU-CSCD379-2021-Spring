namespace SecretSanta.Api.Dto
{
    public class Assignment
    {
        public int Id { get; set; }
        public User? Giver { get; set; }
        public User? Receiver { get; set; }
        public Group? Group { get; set; }

        public static Assignment? ToDto(Data.Assignment? assignment)
        {
            if (assignment is null) return null;
            return new Assignment
            {
                Id = assignment.Id,
                Giver = User.ToDto(assignment.Giver),
                Receiver = User.ToDto(assignment.Receiver),
                Group = Group.ToDto(assignment.Group) // Might need to modify to include children
            };
        }

        public static Data.Assignment? FromDto(Assignment? assignment)
        {
            if (assignment is null) return null;
            return new Data.Assignment
            {
                Id = assignment.Id,
            };
        }
    }
}
