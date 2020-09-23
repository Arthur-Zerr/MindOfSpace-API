using Microsoft.EntityFrameworkCore;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.DAL
{
    public class MindOfSpaceContext : DbContext
    {
        public DbSet<Game> Games{ get; set; }
        public DbSet<Player> Players{ get; set; }
        public DbSet<UserActivity> UserActivities{ get; set; }
        public DbSet<Highscore> Highscores { get; set; }

        public MindOfSpaceContext(DbContextOptions<MindOfSpaceContext> options)
            : base(options)
        {
        }


    }
}