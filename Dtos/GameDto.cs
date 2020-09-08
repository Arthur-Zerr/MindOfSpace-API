using System.Collections.Generic;

namespace MindOfSpace_Api.Controllers.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }
        public int PlayerAmount { get; set; }
        public List<PlayerForGameDto> PlayerList { get; set; }
    }
}