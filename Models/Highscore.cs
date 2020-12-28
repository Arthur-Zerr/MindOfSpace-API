using System;
namespace MindOfSpace_Api.Models
{
    public class Highscore : BaseModel
    {
        public string PlayerId { get; set; }
        public int HighScore { get; set; }
        public DateTimeOffset InstertedDate { get; set; }
        public Player Player { get; set; }
    }
}