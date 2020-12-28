using System.Diagnostics;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MindOfSpace_Api.DAL;
using MindOfSpace_Api.Enums;
using MindOfSpace_Api.Models;
using MindOfSpace_Api.Dtos;

namespace MindOfSpace_Api.Helpers
{
    public class LogUserActivity
    {
        private readonly MindOfSpaceContext MindOfSpaceContext;

        public LogUserActivity (MindOfSpaceContext MindOfSpaceContext)
        {
            this.MindOfSpaceContext = MindOfSpaceContext;

        }

        public async Task<bool> Register(Player player)
        {
            var activity = new UserActivity
            {
                Username = player.UserName,
                GameId = null,
                ActivityType = (int)ActivityType.Register,
                Date = DateTimeOffset.UtcNow
            };
            await this.MindOfSpaceContext.UserActivities.AddAsync (activity);
            return await MindOfSpaceContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> LoginAsync (Player player)
        {
            var activity = new UserActivity
            {
                Username = player.UserName,
                GameId = null,
                ActivityType = (int)ActivityType.Login,
                Date = DateTimeOffset.UtcNow
            };
            await this.MindOfSpaceContext.UserActivities.AddAsync (activity);
            return await this.MindOfSpaceContext.SaveChangesAsync () > 0;
        }

        public async Task<bool> LogoutAsync (Player player)
        {
            var activity = new UserActivity
            {
                Username = player.UserName,
                GameId = null,
                ActivityType = (int)ActivityType.Logout,
                Date = DateTimeOffset.UtcNow
            };
            await this.MindOfSpaceContext.UserActivities.AddAsync (activity);
            return await this.MindOfSpaceContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> GameLogin(Player player, GameDto game)
        {
            var activity = new UserActivity
            {
                Username = player.UserName,
                GameId = game.Id,
                ActivityType = (int)ActivityType.GameLogin,
                Date = DateTimeOffset.UtcNow
            };
            await this.MindOfSpaceContext.UserActivities.AddAsync (activity);
            return await this.MindOfSpaceContext.SaveChangesAsync () > 0;
        }
        
        public async Task<bool> GameLogout(Player player, GameDto game)
        {
            var activity = new UserActivity
            {
                Username = player.UserName,
                GameId = game.Id,
                ActivityType = (int)ActivityType.GameLogout,
                Date = DateTimeOffset.UtcNow
            };
            await this.MindOfSpaceContext.UserActivities.AddAsync (activity);
            return await this.MindOfSpaceContext.SaveChangesAsync () > 0;
        }
    }
}