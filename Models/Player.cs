using System.Collections.Generic;

namespace MindOfSpace_Api.Models
{
    public class Player : BaseModel
    {
        public string  Username { get; set; }
        public string Password { get; set; }
        public string DateSalt { get; set; }
        public int Level { get; set; }
        public List<Highscore> Highscores { get; set; }

    }
}