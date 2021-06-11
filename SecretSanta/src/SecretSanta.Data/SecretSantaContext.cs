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

            /*modelBuilder.Entity<User>()
                //.UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasKey(user => user.Id);
            //modelBuilder.Entity<User>()
                //.HasAlternateKey(user => new { user.FirstName, user.LastName });

            modelBuilder.Entity<Gift>()
                .HasKey(gift => gift.Id);
            //modelBuilder.Entity<Gift>()
                //.HasAlternateKey(gift => new { gift.Title });

            modelBuilder.Entity<Group>()
                //.UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasKey(group => group.Id);
            //modelBuilder.Entity<Group>()  
                //.HasAlternateKey(group => new { group.Name });

            modelBuilder.Entity<Assignment>()
                .HasKey(assign => assign.Id);*/

            modelBuilder.Entity<Group>()
                .HasMany(group => group.Users)
                .WithMany(group => group.Groups);

            modelBuilder.Entity<Group>()
                .HasMany(group => group.Assignments)
                .WithOne(assign => assign.Group);

            modelBuilder.Entity<User>()
                .HasMany(user => user.Gifts)
                .WithOne(user => user.Receiver);
        }
    }
}