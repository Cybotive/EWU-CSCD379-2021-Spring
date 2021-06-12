using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class GroupRepository : IGroupRepository
    {
        private SecretSantaContext Context;

        public GroupRepository(SecretSantaContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public GroupRepository()
        {
            Context = new();
        }

        public Group Create(Group item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            Group? group = Context.Groups.Find(item.Id);
            if (group is not null)
            {
                return group;
            }

            var settledGroup = Context.Groups.Add(item);
            Context.SaveChanges();
            // Likely unnecessary, but ensures Id gets updated from db
            item.Id = settledGroup.CurrentValues.GetValue<int>("Id");

            return item;
        }

        public Group? GetItem(int id)
        {
            //using SecretSantaContext context = new SecretSantaContext();
            
            return Context.Groups
                .Include(group => group.Users)
                .ThenInclude(group => group.Gifts)
                .Include(group => group.Assignments)
                .Where(group => group.Id == id)
                .SingleOrDefault();
        }

        public ICollection<Group> List()
        {
            //using SecretSantaContext context = new SecretSantaContext();
            
            return Context.Groups
                .Include(group => group.Users)
                .ThenInclude(user => user.Gifts)
                .Include(group => group.Assignments)
                .ToList();
        }

        public bool Remove(int id)
        {
            Group? groupToRemove = Context.Groups.Find(id);
            
            if (groupToRemove is not null)
            {
                Context.Groups.Remove(groupToRemove);
                Context.SaveChanges();
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

            Group? foundGroup = GetItem(item.Id);

            if (foundGroup is null)
            {
                return;
            }

            /* Trying to work around tracking bug
            foreach(User user in item.Users.ToList())
            {
                if (foundGroup.Users.Any(u => u.Id == user.Id))
                {
                    item.Users.Remove(user);
                    //item.Users.Remove(user);
                    //foundGroup.Users.Where(u => u.Id == user.Id).SingleOrDefault();
                    //Context.Entry(user).State = EntityState.Unchanged;
                }
            }*/

            Context.Update(item);

            //Context.Groups.Update(item);
            Context.SaveChanges();
        }

        public AssignmentResult GenerateAssignments(int groupId)
        {
            //using SecretSantaContext context = new SecretSantaContext();

            //Group? group = context.Groups.Include(item => item.Users).Where(group => group.Id == groupId).FirstOrDefault();

            Group? group = Context.Groups.Include(item => item.Users).Where(group => group.Id == groupId).FirstOrDefault();

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
                group.Assignments.Add(new Assignment(users[i], users[endIndex], group));
            }

            //context.SaveChanges();
            Context.SaveChanges();
            return AssignmentResult.Success();
        }
    }
}
