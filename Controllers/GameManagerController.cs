using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MindOfSpace_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameManagerController  : ControllerBase
    {
        
        [HttpGet("IsOnline")]
        public async Task<IActionResult> Get()
        {
            return Ok("MindOfGame");
        }


        [HttpPost]
        [Route("Join/{GameId}/{PlayerId}")]
        public async Task<IActionResult> JoinGame(int GameId, int PlayerId)
        {
            return Ok(new {GameId, PlayerId});
        }
        [HttpPost]
        [Route("Leave/{GameId}/{PlayerId}")]
        public async Task<IActionResult> LeaveGame(int GameId, int PlayerId)
        {
            return Ok();
        }


        [HttpGet]
        [Route("Player/{GameId}")]
        public async Task<IActionResult> AllPlayers(int GameId)
        {
            return Ok(GameId);
        }


    }
}