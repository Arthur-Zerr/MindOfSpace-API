using System;
using System.Collections.Generic;

namespace MindOfSpace_Api.Models
{
    public class Game : BaseModel
    {
        public string GameKey { get; set; }
        public DateTimeOffset GameCreated { get; set; }
        public string PlayerHostId { get; set; }
        public Player PlayerHost { get; set; }
    }
}