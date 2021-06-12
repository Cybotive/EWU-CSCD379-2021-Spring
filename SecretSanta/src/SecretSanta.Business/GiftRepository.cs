using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class GiftRepository : IGiftRepository
    {
        private SecretSantaContext Context;

        public GiftRepository(SecretSantaContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public GiftRepository()
        {
            Context = new();
        }

        public Gift Create(Gift item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            Gift? gift = Context.Gifts.Find(item.Id);
            if (gift is not null)
            {
                return gift;
            }

            var settledGift = Context.Gifts.Add(item);
            Context.SaveChanges();
            // Likely unnecessary, but ensures Id gets updated from db
            item.Id = settledGift.CurrentValues.GetValue<int>("Id");

            return item;
        }

        public Gift? GetItem(int id)
        {
            return Context.Gifts
                .Where(gift => gift.Id == id)
                .SingleOrDefault();
        }

        public ICollection<Gift> List()
        {
            return Context.Gifts.ToList();
        }

        public bool Remove(int id)
        {
            Gift? giftToRemove = Context.Gifts.Find(id);
            
            if (giftToRemove is not null)
            {
                Context.Gifts.Remove(giftToRemove);
                Context.SaveChanges();
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

            Gift? foundGift = GetItem(item.Id);

            if (foundGift is null)
            {
                return;
            }

            Context.Gifts.Update(item);
            Context.SaveChanges();
        }
    }
}