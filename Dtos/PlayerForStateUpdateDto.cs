using System.ComponentModel.DataAnnotations;

namespace MindOfSpace_Api.Dtos
{
    public class PlayerForStateUpdateDto
    {
        [Required]
        public string PlayerId { get; set; }

        [Required]
        public string GameId { get; set; }
        
        [Required]
        public string PlayerState{ get; set; }
    }
}