using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.DAL
{
    public class MindOfSpaceRepository
    {
        private readonly MindOfSpaceContext mindOfSpaceContext;

        public MindOfSpaceRepository(MindOfSpaceContext MindOfSpaceContext)
        {
            mindOfSpaceContext = MindOfSpaceContext;
        }

        public async Task<Player> GetPlayerById(int playerid)
        {
            var user = await this.mindOfSpaceContext.Players.Where(x => x.Id == playerid).FirstOrDefaultAsync();
            return user;
        }

        public async Task<Player> GetPlayerByUsername(string Username)
        {
            var user = await this.mindOfSpaceContext.Players.Where(x => x.Username == Username).FirstOrDefaultAsync();
            return user;
        }

        public async Task<Game> GetGameById(int gameId)
        {
            var game = await this.mindOfSpaceContext.Games.Where(x => x.Id == gameId && x.IsDeleted != true).FirstOrDefaultAsync();
            return game;
        }

        public async Task<Game> GetGameByPlayerHost(Player host)
        {
            var game = await this.mindOfSpaceContext.Games.Where(x => x.PlayerHostId == host.Id && x.IsDeleted != true).FirstOrDefaultAsync();

            return game;
        }

        public async Task<List<Player>> GetPlayersByGameId(int gameId)
        {
            var playerList = await this.mindOfSpaceContext.Games.Where(x => x.Id == gameId && x.IsDeleted != true).Select(x => x.Players).FirstOrDefaultAsync();
            return playerList;
        }

        public async Task<bool> DeleteGame(int gameId)
        {
            var game = await this.mindOfSpaceContext.Games.Where(x => x.Id == gameId && x.IsDeleted != true).FirstOrDefaultAsync();

            if(game == null)
                return false;

            game.IsDeleted = true;
            this.mindOfSpaceContext.Update(game);
            return await this.mindOfSpaceContext.SaveChangesAsync() > 0 ? true : false;

        }
    }
}