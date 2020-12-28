using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MindOfSpace_Api.BusinessLogic;
using MindOfSpace_Api.DAL;
using MindOfSpace_Api.Dtos;
using MindOfSpace_Api.Helpers;

namespace MindOfSpace_Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route ("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly MindOfSpaceRepository mindOfSpaceRepository;
        private readonly PlayerLogic playerLogic;
        private readonly PlayerRepository playerRepository;
        private readonly LogUserActivity logUserActivity;

        public PlayerController (IConfiguration configuration, MindOfSpaceRepository mindOfSpaceRepository, PlayerLogic playerLogic, PlayerRepository playerRepository,
                                 LogUserActivity logUserActivity)
        {
            this.playerRepository = playerRepository;
            this.logUserActivity = logUserActivity;
            this.playerLogic = playerLogic;
            this.mindOfSpaceRepository = mindOfSpaceRepository;
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route ("Login")]
        public async Task<IActionResult> Login ([FromBody] PlayerLoginDto playerLoginDto)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var response = await playerLogic.LoginPlayer(playerLoginDto.Username, playerLoginDto.Password);
            if(response != null)
            {
                return Ok(response);
            }
                
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route ("Register")]
        public async Task<IActionResult> Register ([FromBody] PlayerRegisterDto playerRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest ();

            var response = await playerLogic.CreatePlayer(playerRegisterDto.Username, playerRegisterDto.Password);
            if(response != null)
                return Ok(response);

            return BadRequest ();
        }

        [HttpGet ("IsOnline")]
        public IActionResult IsOnline ()
        {
            return Ok ("MindOfGame");
        }
    }
}