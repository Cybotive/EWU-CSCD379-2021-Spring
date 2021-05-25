using System;
using System.Collections.Generic;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class GroupRepository : IGroupRepository
    {
        private Random random { get; } = new(); // Replaced by constructor.

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
        // Algorithm Description: Start with an array of user indexes (relative to group.Users). Incrementing from index 0, randomly select index greater than current index to swap.
        public AssignmentResult GenerateAssignments(int groupId)
        {
            if(MockData.Groups.TryGetValue(groupId, out Group? group))
            {
                if (group is null) { return AssignmentResult.Error(nameof(group) + " may not be null."); }
                
                List<User> users = group.Users;
                if (users.Count <= 2) { return AssignmentResult.Error("Not enough users in group (3+ required)."); }

                List<Assignment> tempAssignments = new();
                int[] recipientIndexes = new int[users.Count];
                
                for (int i = 0; i < recipientIndexes.Length; i++)
                {
                    recipientIndexes[i] = i;
                }

                for (int i = 0; i < users.Count - 1; i++) // -1 to exclude last user (will be included indirectly via swap).
                {
                    int indexToSwap = GetRandomUserIndexRightOfIndex(i);
                    recipientIndexes[indexToSwap] = i;
                    recipientIndexes[i] = indexToSwap;
                }

                for (int i = 0; i < recipientIndexes.Length; i++)
                {
                    Assignment assignment = new(users[i], users[recipientIndexes[i]]);
                    tempAssignments.Add(assignment);
                }

                foreach (Assignment assign in tempAssignments)
                {
                    if (assign.Giver == assign.Receiver) // Equal by reference is intentional.
                    {
                        return AssignmentResult.Error("Unable to generate Secret Santa assignments.");
                    }
                }

                return AssignmentResult.Success();
            }

            return AssignmentResult.Error("Group not found.");
        }

        private int GetRandomUserIndexRightOfIndex(int index)
        {
            if (index >= MockData.Users.Count )
            {
                throw new IndexOutOfRangeException(
                    String.Format("No users right of current index. Current Index: {0}. Max Index: {1}.", index, MockData.Users.Count - 1)
                );
            }
            
            return random.Next(index + 1, MockData.Users.Count);
        }
    }
}
