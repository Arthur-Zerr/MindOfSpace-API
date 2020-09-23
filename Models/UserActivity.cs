using System;

namespace MindOfSpace_Api.Models
{
    public class UserActivity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int GameId { get; set; }
        public DateTimeOffset? LoggedIn { get; set; }
        public DateTimeOffset? LoggedOut { get; set; }
    }
}