using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MindOfSpace_Api.DAL;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.Helpers
{
    public class LogUserActivity
    {
        private readonly MindOfSpaceContext MindOfSpaceContext;

        public LogUserActivity (MindOfSpaceContext MindOfSpaceContext)
        {
            this.MindOfSpaceContext = MindOfSpaceContext;

        }

        public async Task<bool> LoginAsync (Player player, Game game)
        {
            var activity = new UserActivity
            {
                Username = player.Username,
                GameId = game.Id,
                LoggedIn = DateTimeOffset.Now
            };
            await this.MindOfSpaceContext.UserActivities.AddAsync (activity);
            return await this.MindOfSpaceContext.SaveChangesAsync () > 0;
        }

        public async Task<bool> LogoutAsync (Player player, Game game)
        {
            var userActivity = await this.MindOfSpaceContext.UserActivities.Where(x => x.Username == player.Username && x.GameId == game.Id).OrderByDescending(p => p.Id).FirstAsync();

            userActivity.LoggedOut = DateTimeOffset.Now;
            return await this.MindOfSpaceContext.SaveChangesAsync() > 0;
        }
    }
}