using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MindOfSpace_Api.Models
{
    public class Player : IdentityUser
    {
        public DateTimeOffset Date { get; set; }
        public int Level { get; set; }
        public List<Highscore> Highscores { get; set; }
        public bool? IsDeleted { get; set; }
    }
}