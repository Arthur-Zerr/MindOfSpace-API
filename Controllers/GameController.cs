using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindOfSpace_Api.BusinessLogic;
using MindOfSpace_Api.DAL;
using MindOfSpace_Api.Dtos;
using MindOfSpace_Api.Enums;
using MindOfSpace_Api.Helpers;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly LogUserActivity logUserActivity;
        private readonly MindOfSpaceRepository mindOfSpaceRepository;
        private readonly PlayerRepository playerRepository;
        private readonly LobbyHelper lobbyHelper;

        public GameController(LogUserActivity logUserActivity, MindOfSpaceRepository mindOfSpaceRepository, PlayerRepository playerRepository, LobbyHelper lobbyHelper)
        {
            this.playerRepository = playerRepository;
            this.lobbyHelper = lobbyHelper;
            this.logUserActivity = logUserActivity;
            this.mindOfSpaceRepository = mindOfSpaceRepository;
        }

        [HttpGet]
        [Route("IsOnline")]
        public IActionResult IsOnline()
        {
            return Ok("MindOfGame");
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateGame([FromBody] PlayerForGameCreateDto player)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var hostPlayer = await playerRepository.GetPlayerById(player.PlayerId);

            var gameid = lobbyHelper.CreateGame(hostPlayer);
            var game = new Game();
            game.GameKey = gameid;
            game.GameCreated = DateTimeOffset.UtcNow;
            game.PlayerHost = hostPlayer;

            await mindOfSpaceRepository.AddGame(game); 

            var gameLobby = lobbyHelper.GetGameDto(gameid);
            await logUserActivity.GameLogin(hostPlayer, gameLobby);
            return Ok(gameLobby);
        }


        [HttpPost]
        [Route("Join")]
        public async Task<IActionResult> JoinGame([FromBody] PlayerGameDto playerGame)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await playerRepository.GetPlayerById(playerGame.PlayerId);
            var game = lobbyHelper.GetGameDto(playerGame.GameId);

            if(user == null || game == null)
                return BadRequest();

            var response = lobbyHelper.AddPlayerToGame(game.Id, user);

            if(response.Item1)
            {
                await logUserActivity.GameLogin(user, game);
                return Ok(game);
            }
            return BadRequest();

        }
        [HttpPost]
        [Route("Leave")]
        public async Task<IActionResult> LeaveGame([FromBody] PlayerGameDto playerGame)
        {
            if (!ModelState.IsValid)
                return BadRequest();
                
            var user = await playerRepository.GetPlayerById(playerGame.PlayerId);
            var game = lobbyHelper.GetGameDto(playerGame.GameId);

            var response = lobbyHelper.RemovePlayerFromGame(game.Id, user);
            if(response.Item1)
            {
                var result = await logUserActivity.GameLogout(user, game);

                if (result)
                    return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("Game/{gameId}")]
        public IActionResult GetGameUpdate(string gameId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var game = lobbyHelper.GetGameDto(gameId);
            if(game == null)
                return BadRequest();

            return Ok(game);
            
        }

        [HttpPost]
        [Route("UpdatePlayer")]
        public async Task<IActionResult> UpdatePlayerState(PlayerForStateUpdateDto playerForStateUpdateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var user = await playerRepository.GetPlayerById(playerForStateUpdateDto.PlayerId);
            var game = lobbyHelper.GetGameDto(playerForStateUpdateDto.GameId);

            if(user == null || game == null)
                return BadRequest();
            
            PlayerState playerState = PlayerState.Living;
            Enum.TryParse<PlayerState>(playerForStateUpdateDto.PlayerState, out playerState);

            var response = lobbyHelper.UpdatePlayerState(user, game.Id, playerState);
            if(response.Item1)
            {
                game = lobbyHelper.GetGameDto(game.Id);
                var playersToEndGame = (int)Math.Round((decimal)(game.PlayerAmount / 3));
                var endedPlayers = game.PlayerList.Where(x => x.PlayerState != PlayerState.Living).ToList();
                if(endedPlayers.Count >= playersToEndGame)lobbyHelper.UpdateGameState(game.Id, GameState.Ended);
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GameState/{gameId}")]
        public IActionResult GetCurrentGameState(string gameId)
        {
            if(!ModelState.IsValid)
                return BadRequest();
            
            var game = lobbyHelper.GetGameDto(gameId);
            if(game == null)
                return BadRequest();

            return Ok(new GameStateDto {GameState = (int)game.GameState});
        }

        [HttpPost]
        [Route("StartGame")]
        public IActionResult StartGame(GameInformationDto gameInformationDto)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var game = lobbyHelper.GetGameDto(gameInformationDto.GameId);
            if(game == null)
                return BadRequest();

            var response = lobbyHelper.UpdateGameState(gameInformationDto.GameId, Enums.GameState.Started);
            if(response.Item1)
                return Ok(response.Item2);

            return BadRequest();
        }


        [HttpGet]
        [Route("GamePlayers/{gameId}")]
        public IActionResult AllPlayers(string gameId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var playerList = lobbyHelper.PlayerListFromGame(gameId);
            return Ok(playerList);
        }

        [HttpGet]
        [Route("GameExist/{gameId}")]
        public IActionResult GameExist(string gameId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var game = lobbyHelper.GetGameDto(gameId);
            if(game == null)
                return BadRequest();
            if(game.GameState == Enums.GameState.Created)
                return Ok();

            return BadRequest();
        }
    }
}