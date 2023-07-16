namespace LearningSupportSystemAPI;

public class Message : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public MessageType? Type { get; set; } = MessageType.Text;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int DiscussionId { get; set; }
    public Discussion? Discussion { get; set; }

    public string SenderId { get; set; } = string.Empty;
    public User? Sender { get; set; }
}

public enum MessageType
{
    Text,
    Image,
    File
}
