using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class GroupRepository : IGroupRepository
    {
        public Group Create(Group item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            using (SecretSantaContext context = new SecretSantaContext())
            {
                Group group = context.Groups.Find(item.Id);
                if (group is not null)
                {
                    return group;
                }
            }

            using (SecretSantaContext context = new SecretSantaContext())
            {
                context.Groups.Add(item);
                context.SaveChanges();
            }

            return item;
        }

        public Group? GetItem(int id)
        {
            using SecretSantaContext context = new SecretSantaContext();
            return context.Groups.Find(id);
        }

        public ICollection<Group> List()
        {
            using SecretSantaContext context = new SecretSantaContext();
            return context.Groups.AsNoTrackingWithIdentityResolution().ToList();
        }

        public bool Remove(int id)
        {
            using SecretSantaContext context = new SecretSantaContext();
            Group groupToRemove = context.Groups.Find(id);

            if (groupToRemove is not null)
            {
                var tracker = context.Groups.Remove(groupToRemove);
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    tracker.State = EntityState.Unchanged;
                    return false;
                }
                
                return true;
            }
            
            return false;
        }

        public void Save(Group item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            using (SecretSantaContext context = new SecretSantaContext())
            {
                if (context.Groups.Find(item.Id) is null)
                {
                    return;
                }
            }

            using (SecretSantaContext context = new SecretSantaContext())
            {
                context.Groups.Update(item);
                context.SaveChanges();
            }
        }

        public AssignmentResult GenerateAssignments(int groupId)
        {
            using SecretSantaContext context = new SecretSantaContext();

            Group group = context.Groups.Find(groupId);

            if (group is null)
            {
                return AssignmentResult.Error("Group not found");
            }

            Random random = new();
            var groupUsers = new List<User>(group.Users);

            if (groupUsers.Count < 3)
            {
                return AssignmentResult.Error($"Group {group.Name} must have at least three users");
            }

            var users = new List<User>();
            //Put the users in a random order
            while(groupUsers.Count > 0)
            {
                int index = random.Next(groupUsers.Count);
                users.Add(groupUsers[index]);
                groupUsers.RemoveAt(index);
            }

            //The assignments are created by linking the current user to the next user.
            group.Assignments.Clear();
            for(int i = 0; i < users.Count; i++)
            {
                int endIndex = (i + 1) % users.Count;
                group.Assignments.Add(new Assignment(users[i], users[endIndex]));
            }
            return AssignmentResult.Success();
        }
    }
}
