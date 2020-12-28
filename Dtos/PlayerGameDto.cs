using System.ComponentModel.DataAnnotations;

namespace MindOfSpace_Api.Dtos
{
    public class PlayerGameDto
    {
        [Required]
        public string PlayerId { get; set; }
        
        [Required]
        public string GameId { get; set; }
    }
}