namespace LearningSupportSystemAPI;

public class Discussion : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DiscussionType Type { get; set; } = DiscussionType.Question;
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public int? GroupId { get; set; }
    public Group? Group { get; set; }

    public int ClassId { get; set; }
    public Class? Class { get; set; }

    public string? CreatorId { get; set; }
    public User? Creator { get; set; }

    public ICollection<Message>? Messages { get; set; }
    public ICollection<User> Participants { get; set; } = new HashSet<User>();
}

public enum DiscussionType
{
    Question,
    Chat
}