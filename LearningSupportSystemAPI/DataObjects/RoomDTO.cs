namespace LearningSupportSystemAPI;

public class RoomDTO : BaseDTO
{
    public string Code { get; set; } = string.Empty;
    public RoomType? Type { get; set; }
    public string? Building { get; set; } = string.Empty;
    public int? Seat { get; set; }

    public DepartmentDTO? Department { get; set; }
    public virtual ICollection<ClassDTO>? Classes { get; set; }
}

public class CreateRoomDTO
{
    public string Code { get; set; } = string.Empty;
    public RoomType? Type { get; set; }
    public string? Building { get; set; } = string.Empty;
    public int? Seat { get; set; }

    public int? DepartmentId { get; set; }
}
