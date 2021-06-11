﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecretSanta.Data
{
    public class Assignment
    {
        public int Id { get; set; }
        public User Giver { get; set; }
        public User Receiver { get; set; }
        public Group Group {get; set; }

        public Assignment(User giver, User receiver, Group group)
        {
            Giver = giver ?? throw new ArgumentNullException(nameof(giver));
            Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
            Group = group ?? throw new ArgumentNullException(nameof(group));
        }

        private Assignment() // Currently unavoidable null-path due to EF weirdness
        {}
    }
}