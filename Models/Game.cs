using System;
using System.Collections.Generic;

namespace MindOfSpace_Api.Models
{
    public class Game : BaseModel
    {
        public DateTimeOffset GameCreated { get; set; }
        public int PlayerHostId { get; set; }
        public Player PlayerHost { get; set; }
        public List<Player> Players { get; set; }

    }
}