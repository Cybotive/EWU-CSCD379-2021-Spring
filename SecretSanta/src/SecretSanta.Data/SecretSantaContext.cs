using System;
using Microsoft.EntityFrameworkCore;
using  Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecretSanta.Data
{
    public class SecretSantaContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Gift> Gifts => Set<Gift>();

        public SecretSantaContext() : base(new DbContextOptionsBuilder<SecretSantaContext>()
            .EnableSensitiveDataLogging().UseSqlite("Data Source=main.db").Options)
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder is null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            /* These are making my tests fail due to bad configuration, but otherwise they work...
            modelBuilder.Entity<User>()
                .HasAlternateKey(user => new { user.FirstName, user.LastName });

            modelBuilder.Entity<Gift>()
                .HasAlternateKey(gift => new { gift.Title });

            modelBuilder.Entity<Group>()  
                .HasAlternateKey(group => new { group.Name });*/
        }
    }
}