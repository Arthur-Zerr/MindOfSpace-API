using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MindOfSpace_Api.DAL;
using MindOfSpace_Api.Dtos;

namespace MindOfSpace_Api.Controllers
{
    public class HighscoreController : ControllerBase
    {
        private readonly HighscoreRepository highscoreRepository;
        private readonly MindOfSpaceRepository mindOfSpaceRepository;
        public HighscoreController (HighscoreRepository highscoreRepository, MindOfSpaceRepository mindOfSpaceRepository)
        {
            this.mindOfSpaceRepository = mindOfSpaceRepository;
            this.highscoreRepository = highscoreRepository;
        }

        [HttpPost]
        [Route ("Highscore")]
        public async Task<IActionResult> HighScore (HighscoreDto highScoreDto)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var player = await mindOfSpaceRepository.GetPlayerById(highScoreDto.PlayerId);

            if(player == null)
                return BadRequest();

            var result = await highscoreRepository.AddHighscore(player, highScoreDto.HighScore);
            
            if(result)
                return Ok();

            return BadRequest ();
        }

        [HttpGet]
        [Route ("LastHighScore")]
        public async Task<IActionResult> LastHighScore (int PlayerId)
        {
            var player = await mindOfSpaceRepository.GetPlayerById(PlayerId);
            if(player == null)
                return BadRequest();

            var highscore = await highscoreRepository.GetLastHighscore(player);

            return Ok (highscore);
        }

        [HttpGet]
        [Route ("Highscores")]
        public async Task<IActionResult> Highscores (int PlayerId)
        {
            var player = await mindOfSpaceRepository.GetPlayerById(PlayerId);
            if(player == null)
                return BadRequest();

            var highscores = await highscoreRepository.GetHighscores(player);

            return Ok (highscores);
        }

    }
}