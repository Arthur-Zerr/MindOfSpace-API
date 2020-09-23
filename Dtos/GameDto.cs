using System.Collections.Generic;
using MindOfSpace_Api.Enums;
using MindOfSpace_Api.Models;

namespace MindOfSpace_Api.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }
        public Player PlayerHost { get; set; }
        public GameState GameState { get; set; }
        public int PlayerAmount { get; set; }
        public List<PlayerForGameDto> PlayerList { get; set; }
    }
}