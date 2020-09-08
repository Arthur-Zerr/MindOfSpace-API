using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MindOfSpace_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerManagerController : ControllerBase
    {
        [HttpGet("IsOnline")]
        public async Task<IActionResult> Get()
        {
            return Ok("MindOfGame");
        }
    }
}