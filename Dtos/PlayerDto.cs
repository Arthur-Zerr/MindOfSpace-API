using System;

namespace MindOfSpace_Api.Dtos
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTimeOffset lastActive { get; set; }
    }
}