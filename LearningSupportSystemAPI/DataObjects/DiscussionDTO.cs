namespace LearningSupportSystemAPI;

public class DiscussionDTO : BaseDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DiscussionType Type { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public int? GroupId { get; set; }
    public int ClassId { get; set; }

    public string? CreatorId { get; set; }
    public UserDTO? Creator { get; set; }

    public ICollection<MessageDTO>? Messages { get; set; }
    public ICollection<UserDTO> Participants { get; set; } = new HashSet<UserDTO>();
}

public class CreateDiscussionDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DiscussionType? Type { get; set; } = DiscussionType.Question;
    public int? GroupId { get; set; }
    public int ClassId { get; set; }
    public string? CreatorId { get; set; }
    
}
