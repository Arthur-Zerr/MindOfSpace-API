using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MindOfSpace_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public PlayerController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login()
        {
            return BadRequest();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register()
        {
            return BadRequest();
        }

        [HttpGet("IsOnline")]
        public async Task<IActionResult> Get()
        {
            return Ok("MindOfGame");
        }
    }
}