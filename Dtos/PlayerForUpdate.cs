namespace MindOfSpace_Api.Dtos
{
    public class PlayerForUpdate
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }
        public int MyProperty { get; set; }
    }
}