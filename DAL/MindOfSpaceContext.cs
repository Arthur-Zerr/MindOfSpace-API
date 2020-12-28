using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.DAL
{
    public class MindOfSpaceContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Game> Games{ get; set; }
        public DbSet<Player> Players{ get; set; }
        public DbSet<UserActivity> UserActivities{ get; set; }
        public DbSet<Highscore> Highscores { get; set; }

        public MindOfSpaceContext(DbContextOptions<MindOfSpaceContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("Player");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
        }


    }
}