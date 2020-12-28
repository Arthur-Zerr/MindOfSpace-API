using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindOfSpace_Api.DAL;
using MindOfSpace_Api.Dtos;

namespace MindOfSpace_Api.Controllers
{
    [Authorize(Roles = "Player, Admin")]
    public class HighscoreController : ControllerBase
    {
        private readonly HighscoreRepository highscoreRepository;
        private readonly MindOfSpaceRepository mindOfSpaceRepository;
        private readonly PlayerRepository playerRepository;
        public HighscoreController(HighscoreRepository highscoreRepository, MindOfSpaceRepository mindOfSpaceRepository, PlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
            this.mindOfSpaceRepository = mindOfSpaceRepository;
            this.highscoreRepository = highscoreRepository;
        }

        [HttpPost]
        [Route("Highscore")]
        public async Task<IActionResult> HighScore(HighscoreDto highScoreDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var player = await playerRepository.GetPlayerById(highScoreDto.PlayerId);

            if (player == null)
                return BadRequest();

            var result = await highscoreRepository.AddHighscore(player, highScoreDto.HighScore);

            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpGet]
        [Route("LastHighScore")]
        public async Task<IActionResult> LastHighScore(string PlayerId)
        {
            var player = await playerRepository.GetPlayerById(PlayerId);
            if (player == null)
                return BadRequest();

            var highscore = await highscoreRepository.GetLastHighscore(player);

            return Ok(highscore);
        }

        [HttpGet]
        [Route("Highscores")]
        public async Task<IActionResult> Highscores(string PlayerId)
        {
            var player = await playerRepository.GetPlayerById(PlayerId);
            if (player == null)
                return BadRequest();

            var highscores = await highscoreRepository.GetHighscores(player);

            var highscoreList = highscores.Select(x => new PlayerHighscoreDto { HighScore = x.HighScore}).ToList();

            return Ok(highscoreList);
        }

    }
}