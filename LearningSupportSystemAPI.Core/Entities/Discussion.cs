﻿namespace LearningSupportSystemAPI.Core.Entities
{
    public class Discussion : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public int? Type { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Message>? Messages { get; set; }
        public ICollection<User> Participants { get; set; } = new HashSet<User>();
    }
}
