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
                Group? exisitingGroup = context.Groups
                    .Where(group => group.Id == item.Id)
                    .Include(group => group.Users)
                    .SingleOrDefault();

                if (exisitingGroup is null)
                {
                    return;
                }

                //context.Users.Remove Might just need to create a UsersGroups many-to-many object manually to solve the issue

                context.Entry<Group>(exisitingGroup).CurrentValues.SetValues(item);

                foreach (User existingUser in exisitingGroup.Users)
                {
                    if(!item.Users.Any(user => user.Id == existingUser.Id))
                    {
                        context.Users.Remove(existingUser);
                    }
                }

                foreach (User updatedUser in item.Users)
                {
                    User? existingUser = exisitingGroup.Users
                        .Where(user => user.Id == updatedUser.Id)
                        .SingleOrDefault();

                    if (existingUser is null)
                    {
                        exisitingGroup.Users.Add(updatedUser);
                        context.SaveChanges();
                    }
                    else
                    {
                        context.Entry(existingUser).CurrentValues.SetValues(updatedUser);
                    }
                }

                //context.Groups.Update(item);
                context.SaveChanges();
            }
        }

        public AssignmentResult GenerateAssignments(int groupId)
        {
            using SecretSantaContext context = new SecretSantaContext();

            Group? group = context.Groups.Include(item => item.Users).Where(group => group.Id == groupId).FirstOrDefault();

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

            context.SaveChanges();
            return AssignmentResult.Success();
        }
    }
}
