using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class GiftRepository : IGiftRepository
    {
        public Gift Create(Gift item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            using (SecretSantaContext context = new SecretSantaContext())
            {
                Gift? gift = context.Gifts.Find(item.Id);
                if (gift is not null)
                {
                    return gift;
                }
            }

            using (SecretSantaContext context = new SecretSantaContext())
            {
                var settledGift = context.Add(item);
                context.SaveChanges();
                // Likely unnecessary, but ensures Id gets updated from db
                item.Id = settledGift.CurrentValues.GetValue<int>("Id");
            }

            return item;
        }

        public Gift? GetItem(int id)
        {
            using SecretSantaContext context = new SecretSantaContext();

            return context.Gifts
                .Where(gift => gift.Id == id)
                .SingleOrDefault();
        }

        public ICollection<Gift> List()
        {
            using SecretSantaContext context = new SecretSantaContext();

            return context.Gifts.ToList();
        }

        public bool Remove(int id)
        {
            using SecretSantaContext context = new SecretSantaContext();
            Gift? giftToRemove = context.Gifts.Find(id);
            
            if (giftToRemove is not null)
            {
                var tracker = context.Gifts.Remove(giftToRemove);
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

        public void Save(Gift item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            using (SecretSantaContext context = new SecretSantaContext())
            {
                if (context.Gifts.Find(item.Id) is null)
                {
                    return;
                }
            }

            using (SecretSantaContext context = new SecretSantaContext())
            {
                context.Gifts.Update(item);
                context.SaveChanges();
            }
        }
    }
}