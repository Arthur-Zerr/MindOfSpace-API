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

        public async Task<bool> AddGame(Game game)
        {
            await mindOfSpaceContext.Games.AddAsync(game);
            return await mindOfSpaceContext.SaveChangesAsync() > 0;
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