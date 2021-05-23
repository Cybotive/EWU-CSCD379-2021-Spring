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
        public AssignmentResult GenerateAssignments(int groupId)
        {
            if(MockData.Groups.TryGetValue(groupId, out Group? group))
            {
                if (group is null) { return AssignmentResult.Error(nameof(group) + " may not be null."); }
                
                List<User> users = group.Users;

                if (users.Count <= 2)
                {

                }
            }

            throw new NotImplementedException();
        }
    }
}
