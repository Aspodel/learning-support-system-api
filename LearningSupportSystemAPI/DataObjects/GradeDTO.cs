namespace LearningSupportSystemAPI;

public class GradeDTO : BaseDTO
{
    public int? Value { get; set; }

    public int GradeColumnId { get; set; }
    public string StudentId { get; set; } = string.Empty;
}
public class GradeRowDTO
{
    public string Id { get; set; } = string.Empty;
    public string Student { get; set; } = string.Empty;
    public Dictionary<string, int?> Grades { get; set; }
}

public class StudentGradesDTO
{
    public string Course { get; set; } = string.Empty;
    public Dictionary<string, int?> Grades { get; set; }
}
