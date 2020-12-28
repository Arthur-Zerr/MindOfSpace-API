namespace MindOfSpace_Api.Dtos
{
    public class PlayerForUpdate
    {
        public string PlayerId { get; set; }
        public int GameId { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }
    }
}