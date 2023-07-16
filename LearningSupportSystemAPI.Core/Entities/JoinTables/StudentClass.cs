namespace LearningSupportSystemAPI;
public class StudentClass
{
    public string StudentId { get; set; } = string.Empty;
    public Student? Student { get; set; }

    public int ClassId { get; set; }
    public Class? Class { get; set; }

    public int? GroupId { get; set; }
    public Group? Group { get; set; }

    public PositionType? Position { get; set; }
}

public enum PositionType
{
    Member,
    Leader
}
