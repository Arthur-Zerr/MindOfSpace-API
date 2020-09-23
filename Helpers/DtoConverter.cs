using System;
using MindOfSpace_Api.Dtos;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.Helpers
{
    public static class DtoConverter
    {
        public static PlayerForGameDto ToPlayerForGameDto(this Player player)
        {
            var tempDto = new PlayerForGameDto();
            tempDto.Id = player.Id;
            tempDto.Level = player.Level;
            tempDto.Username = player.Username;

            return tempDto;
        }

        public static PlayerGameDto ToPlayerGameDto(this Player player, Game game)
        {
            var tempDto = new PlayerGameDto();
            tempDto.PlayerId = player.Id;
            tempDto.GameId = game.Id;

            return tempDto;
        }

        public static PlayerDto ToPlayerDto(this Player player)
        {
            var tempDto = new PlayerDto();
            tempDto.Id = player.Id;
            tempDto.lastActive = DateTimeOffset.Now; // TODO: Maybe Change to Better shit
            tempDto.Username = player.Username;

            return tempDto;
        }
    }
}