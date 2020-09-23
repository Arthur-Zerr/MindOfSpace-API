using System.Collections.Generic;
using System.Threading.Tasks;
using MindOfSpace_Api.Dtos;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.BusinessLogic
{
    public class GameLogic
    {
        public static List<GameDto> Games = new List<GameDto>();

        public async Task<int> CreateGame(Player host)
        {
            var tempGame = new GameDto();
            tempGame.PlayerHost = host;
            
            return 0;
        }

        public async Task<bool> JoinGame(Player join, Game game)
        {
            return false;
        }

        public async Task<bool> DeleteGame(Game game, Player host)
        {
            return false;
        }
    }
}