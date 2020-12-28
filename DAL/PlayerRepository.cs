using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.DAL
{
    public class PlayerRepository
    {
        private readonly MindOfSpaceContext mindOfSpaceContext;

        public PlayerRepository(MindOfSpaceContext mindOfSpaceContext)
        {
            this.mindOfSpaceContext = mindOfSpaceContext;
        }

        public async Task<Player> GetPlayerById(string playerid)
        {
            var user = await this.mindOfSpaceContext.Players.Where(x => x.Id == playerid).FirstOrDefaultAsync();
            return user;
        }

        public async Task<Player> GetPlayerByUsername(string Username)
        {
            var user = await this.mindOfSpaceContext.Players.Where(x => x.UserName.ToUpper() == Username.ToUpper()).FirstOrDefaultAsync();
            return user;
        }


        public async Task<bool> DeletePlayer(string username)
        {
            var temp = await GetPlayerByUsername(username);
            if(temp == null)
                return false;

            var player = await GetPlayerByUsername(username);
            player.IsDeleted = true;

            mindOfSpaceContext.Update(player);

            return await mindOfSpaceContext.SaveChangesAsync() > 0 ? true : false;
        }
    }
}