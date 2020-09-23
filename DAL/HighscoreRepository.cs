using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.DAL
{
    public class HighscoreRepository
    {
        private readonly MindOfSpaceContext mindOfSpaceContext;

        public HighscoreRepository(MindOfSpaceContext MindOfSpaceContext)
        {
            mindOfSpaceContext = MindOfSpaceContext;
        }

        public async Task<int> GetLastHighscore(Player player)
        {
            var temp = await mindOfSpaceContext.Highscores.Where(x => x.PlayerId == player.Id && x.IsDeleted != true).OrderByDescending(x => x.InstertedDate).FirstOrDefaultAsync();
            return temp.HighScore;
        }

        public async Task<List<Highscore>> GetHighscores(Player player)
        {
            var list = await mindOfSpaceContext.Highscores.Where(x => x.PlayerId == player.Id && x.IsDeleted != true).ToListAsync();
            
            return list;
        }


        public async Task<bool> AddHighscore(Player player, int Highscore) 
        {
            var temp = new Highscore();
            temp.InstertedDate = DateTimeOffset.Now;
            temp.PlayerId = player.Id;
            temp.HighScore = Highscore;

            await mindOfSpaceContext.Highscores.AddAsync(temp);
            
            return await mindOfSpaceContext.SaveChangesAsync() > 0 ? true : false ;
        }

        public async Task<bool> DeleteHighscore (int highscoreId)
        {
            var temp = await mindOfSpaceContext.Highscores.Where(x => x.Id == highscoreId).FirstOrDefaultAsync();
            temp.IsDeleted = true;

            mindOfSpaceContext.Update(temp);


            return await mindOfSpaceContext.SaveChangesAsync() > 0 ? true : false;
        }
    }
}