using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MindOfSpace_Api.BusinessLogic;
using MindOfSpace_Api.DAL;
using MindOfSpace_Api.Dtos;
using MindOfSpace_Api.Helpers;

namespace MindOfSpace_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController  : ControllerBase
    {
        private readonly LogUserActivity logUserActivity;
        private readonly MindOfSpaceRepository mindOfSpaceRepository;
        private readonly GameLogic gameLogic;

        public GameController(LogUserActivity logUserActivity, MindOfSpaceRepository mindOfSpaceRepository, GameLogic gameLogic)
        {
            this.logUserActivity = logUserActivity;
            this.mindOfSpaceRepository = mindOfSpaceRepository;
            this.gameLogic = gameLogic;
        }


        [HttpGet]
        [Route("IsOnline")]
        public async Task<IActionResult> Get()
        {
            return Ok("MindOfGame");
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateGame([FromBody] int PlayerId)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var hostPlayer = await mindOfSpaceRepository.GetPlayerById(PlayerId);
            
            return Ok();
        }


        [HttpPost]
        [Route("Join")]
        public async Task<IActionResult> JoinGame([FromBody] PlayerGameDto playerGame)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var user = await mindOfSpaceRepository.GetPlayerById(playerGame.PlayerId);
            var game = await mindOfSpaceRepository.GetGameById(playerGame.GameId);

            await logUserActivity.LoginAsync(user, game);

            return Ok();
        }
        [HttpPost]
        [Route("Leave")]
        public async Task<IActionResult> LeaveGame([FromBody] PlayerGameDto playerGame)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var user = await mindOfSpaceRepository.GetPlayerById(playerGame.PlayerId);
            var game = await mindOfSpaceRepository.GetGameById(playerGame.GameId);

            if(game.PlayerHostId == user.Id)
                if(!await mindOfSpaceRepository.DeleteGame(game.Id))
                    return BadRequest();

            var result = await logUserActivity.LogoutAsync(user, game);

            if(result)
                return Ok();

            return BadRequest();
        }


        [HttpGet]
        [Route("Player/{GameId}")]
        public async Task<IActionResult> AllPlayers(int GameId)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var playerList = await mindOfSpaceRepository.GetPlayersByGameId(GameId);
            return Ok(playerList);
        }



        [HttpPost]
        [Route("Player/{GameId}")]

        public async Task<IActionResult> UpdatePlayerData([FromBody] PlayerForUpdate playerForUpdate)
        {
            return BadRequest();
        }
    }
}