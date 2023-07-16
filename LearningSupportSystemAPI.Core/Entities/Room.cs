namespace LearningSupportSystemAPI;

public class Room : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public RoomType? Type { get; set; } = RoomType.Normal;
    public string? Building { get; set; } = string.Empty;
    public int? Seat { get; set; }

    public Department? Department { get; set; }

    public ICollection<Class>? Classes { get; set; }
}

public enum RoomType
{
    Normal,
    Lab,
    Office
}
