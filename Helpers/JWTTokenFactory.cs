using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MindOfSpace_Api.Dtos;
using MindOfSpace_Api.Models;

namespace medsurv_diary_apigateway.Helpers
{
    public class JWTTokenFactory
    {
        /// <summary>
        /// Generates the token with claims for the user that is logging in. The configuration is from the settings.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userManager"></param>
        /// <param name="config"></param>
        /// /// <returns></returns>
        public async Task<TokenInformationDto> GenerateJwtTokenAsync(Player user, UserManager<Player> userManager, IConfiguration config)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:JwtKey").Value));
            var expires = DateTime.Now.AddHours(Convert.ToDouble(config.GetSection("AppSettings:JwtExpireHours").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenInformationDto { JWTToken = tokenHandler.WriteToken(token), Expires = expires };
        }
    }
}