using System;
using System.Collections.Generic;
using System.Text;
using DUBle_Watch.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DUBle_Watch.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Anime> Anime { get; set; }
        public DbSet<AnimeTracked> AnimeTracked { get; set; }

        public DbSet<Genre> Genre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Anime>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("GETDATE()");

            
            // Create a new user for Identity Framework
            ApplicationUser user = new ApplicationUser
            {
                FirstName = "admin",
                LastName = "admin",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Admin8*");
            modelBuilder.Entity<ApplicationUser>().HasData(user);

            // Create two cohorts
            modelBuilder.Entity<Anime>().HasData(
                new Anime()
                {
                    AnimeId = 1,
                    Name = "Black Clover",
                    CurrentLastEpisode = 88,
                    GenreId = 2,
                    AnimeLink = "https://www.crunchyroll.com/black-clover",
                    Description = "anime about a dude who has no wizard powers but gets strong",
                    hasAnimeEnded = false
                },
               new Anime()
               {
                   AnimeId = 2,
                   Name = "Naruto",
                   CurrentLastEpisode = 1088,
                   GenreId = 1,
                   AnimeLink = "https://www.crunchyroll.com/naruto",
  Description = "anime about a dude who has no wizard powers but gets strong",
                   hasAnimeEnded = false
               }
            );

            modelBuilder.Entity<AnimeTracked>().HasData(
                new AnimeTracked()
                {
                    AnimeTrackedId = 1,

                    AnimeId = 1,

                    TimesCompleted = 0,

                    IsInCurrentlyCompletedSection = false,

                    CurrentEpisode = 10

                },
                new AnimeTracked()
                {
                    AnimeTrackedId = 2,

                    AnimeId = 2,

                    IsInCurrentlyCompletedSection = false,

                    CurrentEpisode = 10
                }
            );

            modelBuilder.Entity<Genre>().HasData(
                new Genre()
                {

                     GenreId = 1,

                     Name = "Drama"

                },
                new Genre()
                {
                    GenreId = 2,
                    Name = "Action"
                }
            );
        }
    }
}
