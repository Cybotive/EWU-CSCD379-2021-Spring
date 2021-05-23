using System;
using System.Collections.Generic;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class GroupRepository : IGroupRepository
    {
        private Random random { get; }

        public GroupRepository(Random random)
        {
            random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public Group Create(Group item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            MockData.Groups[item.Id] = item;
            return item;
        }

        public Group? GetItem(int id)
        {
            if (MockData.Groups.TryGetValue(id, out Group? group))
            {
                return group;
            }
            return null;
        }

        public ICollection<Group> List()
        {
            return MockData.Groups.Values;
        }

        public bool Remove(int id)
        {
            return MockData.Groups.Remove(id);
        }

        public void Save(Group item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            MockData.Groups[item.Id] = item;
        }

        /*
        * A group with with 2 or fewer users should result in an error. This error should be displayed to a user.
        * A user is not allowed to be both the Giver and Recipient of the assignment.
        */
        // Algorithm Description: Start with an array of users. Incrementing from index 0, randomly select index greater than current index to conceptually swap.
        public AssignmentResult GenerateAssignments(int groupId)
        {
            if(MockData.Groups.TryGetValue(groupId, out Group? group))
            {
                if (group is null) { return AssignmentResult.Error(nameof(group) + " may not be null."); }
                
                List<User> users = group.Users;
                List<Assignment> tempAssignments = new();

                if (users.Count <= 2)
                {
                    return AssignmentResult.Error("Not enough users in group (3+ required).");
                }

                for (int i = 0; i < users.Count - 1; i++) // -1 to exclude last user
                {
                    User? recipient = GetRandomUserRightOfIndex(i);
                    if (recipient is null) { return AssignmentResult.Error("Invalid recipient."); } // May not want the user to know this, but I do for this assignment.

                    Assignment assignment = new(users[i], recipient);
                    //group.Assignments.Add(assignment);
                    tempAssignments.Add(assignment);
                }

                foreach (Assignment assign in tempAssignments)
                {
                    if (assign.Giver == assign.Receiver) // Equal by reference is intentional.
                    {
                        return AssignmentResult.Error("Unable to generate Secret Santa assignments.");
                    }
                }
            }

            return AssignmentResult.Error("Group not found.");
        }

        /*private User? GetRandomUser()
        {
            if (MockData.Users.Count <= 0 ) { return null; }

            Random random = new();
            
            User user = MockData.Users[random.Next(MockData.Users.Count - 1)];

            return user;
        }*/

        private User? GetRandomUserRightOfIndex(int index)
        {
            if (index >= MockData.Users.Count )
            {
                throw new IndexOutOfRangeException(
                    String.Format("No users right of current index. Current Index: {0}. Max Index: {1}.", index, MockData.Users.Count - 1)
                );
            }
            
            Random random = new();
            
            User user = MockData.Users[random.Next(index + 1, MockData.Users.Count - 1)];
            return user;
        }
    }
}
