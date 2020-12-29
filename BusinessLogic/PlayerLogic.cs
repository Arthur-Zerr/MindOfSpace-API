using System;
using System.Threading.Tasks;
using medsurv_diary_apigateway.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MindOfSpace_Api.DAL;
using MindOfSpace_Api.Dtos;
using MindOfSpace_Api.Helpers;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.BusinessLogic
{
    public class PlayerLogic
    {
        private readonly MindOfSpaceRepository MindOfSpaceRepository;
        private readonly PlayerRepository playerRepository;
        private readonly UserManager<Player> userManager;
        private readonly SignInManager<Player> signInManager;
        private readonly JWTTokenFactory tokenFactory;
        private readonly IConfiguration configuration;
        private readonly LogUserActivity logUserActivity;

        public PlayerLogic(MindOfSpaceRepository MindOfSpaceRepository, PlayerRepository playerRepository, UserManager<Player> userManager, SignInManager<Player> signInManager, JWTTokenFactory tokenFactory, IConfiguration configuration, LogUserActivity logUserActivity)
        {
            this.playerRepository = playerRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenFactory = tokenFactory;
            this.configuration = configuration;
            this.logUserActivity = logUserActivity;
            this.MindOfSpaceRepository = MindOfSpaceRepository;
        }


        public async Task<PlayerForReturnDto> CreatePlayer(string username, string password)
        {
            var player = await userManager.FindByNameAsync(username);
            if(player != null)
                return null;
            
            var newPlayer = new Player{ UserName = username, Date = DateTime.UtcNow }; 

            var result = await userManager.CreateAsync(newPlayer, password);
            if(result.Succeeded)
            {
                var roleResult = await userManager.AddToRoleAsync(newPlayer, "User");
                
                var tokenInfo = await tokenFactory.GenerateJwtTokenAsync(newPlayer, userManager, configuration);
                await logUserActivity.Register(newPlayer);

                return newPlayer.ToPlayerReturnDto(tokenInfo);
            }
            
            return null;
        }

        public async Task<PlayerForReturnDto> LoginPlayer(string username, string password)
        {
            var player = await userManager.FindByNameAsync(username);
            if(player == null)
                return null;

            var signInResult = await signInManager.CheckPasswordSignInAsync(player, password, false);

            if(signInResult.Succeeded)
            {
                var tokenInfo = await tokenFactory.GenerateJwtTokenAsync(player, userManager, configuration);
                await logUserActivity.LoginAsync(player);
                return player.ToPlayerReturnDto(tokenInfo);
            }

            return null;
        }


    }
}