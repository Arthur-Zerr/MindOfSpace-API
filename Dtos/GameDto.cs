using System.Collections.Generic;
using MindOfSpace_Api.Enums;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.Dtos
{
    public class GameDto
    {
        public string Id { get; set; }
        public PlayerForGameDto PlayerHost { get; set; }
        public GameState GameState { get; set; }
        public int PlayerAmount => PlayerList?.Count ?? 0;
        public List<PlayerForGameDto> PlayerList { get; set; }
    }
}