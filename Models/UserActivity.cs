using System;

namespace MindOfSpace_Api.Models
{
    public class UserActivity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? GameId { get; set; }
        public int ActivityType { get; set; }
        public DateTimeOffset? Date { get; set; }
    }
}