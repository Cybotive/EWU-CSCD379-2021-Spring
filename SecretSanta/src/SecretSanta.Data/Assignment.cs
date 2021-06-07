using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecretSanta.Data
{
    public class Assignment
    {
        public int Id { get; set; }
        public User Giver { get; private set; }
        public User Receiver { get; private set; }
        /*public string ForeignKey
        {
            get {
                return $"{Giver.FirstName} {Giver.LastName} {Receiver.FirstName} {Receiver.LastName}";
            }
        }*/

        public Assignment(User giver, User receiver)
        {
            Giver = giver ?? throw new ArgumentNullException(nameof(giver));
            Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        }

        public Assignment()
        {
            if(Giver is null)
                throw new ArgumentNullException(nameof(Giver));

            if(Receiver is null)
                throw new ArgumentNullException(nameof(Receiver));
        }
    }
}
