using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.DataObjects
{
    public class MessageDTO : BaseDTO
    {
        public string Content { get; set; } = string.Empty;
        public MessageType? Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int DiscussionId { get; set; }
        public string SenderId { get; set; } = string.Empty;
    }
}
