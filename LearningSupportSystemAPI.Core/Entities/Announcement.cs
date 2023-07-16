namespace LearningSupportSystemAPI;

public class Announcement : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public AnnouncementType? Type { get; set; } = AnnouncementType.School;

    public string SenderId { get; set; } = string.Empty;
    public User? Sender { get; set; }
}

public enum AnnouncementType
{
    School,
    Department,
    Advisor,
    Class
}
