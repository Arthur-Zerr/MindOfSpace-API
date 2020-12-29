using MindOfSpace_Api.Enums;

namespace MindOfSpace_Api.Dtos
{
    public class PlayerForGameDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public int Level { get; set; }
        public PlayerState PlayerState { get; set; } = PlayerState.Living;
    }
}