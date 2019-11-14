using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Models;

namespace User.API.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .ToTable("Users")
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserProperty>().Property(x => x.Key).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>().Property(x => x.Value).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>()
                .ToTable("UserProperties")
                .HasKey(u => new { u.Key, u.AppUserId, u.Value });

            modelBuilder.Entity<UserTage>().Property(x => x.Tag).HasMaxLength(100);
            modelBuilder.Entity<UserTage>()
                .ToTable("UserTages")
                .HasKey(u => new { u.UserId, u.Tag });

            modelBuilder.Entity<BpFile>()
               .ToTable("UserTages")
               .HasKey(u => u.Id);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
