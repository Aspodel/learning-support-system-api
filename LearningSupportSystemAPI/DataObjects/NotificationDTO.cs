using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.DataObjects
{
    public class NotificationDTO : BaseDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public NotificationType? Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool? IsRead { get; set; } = false;
        public bool? IsDeleted { get; set; } = false;

        public string RecipientId { get; set; } = string.Empty;
    }
}
