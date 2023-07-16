namespace LearningSupportSystemAPI;

public class GradeColumnDTO : BaseDTO
{
    public string Name { get; set; } = string.Empty;
    public bool? IsPublished { get; set; } = false;
    public int Percentage { get; set; }
    public int? Order { get; set; }

    public int ClassId { get; set; }
    public ICollection<GradeDTO>? Grades { get; set; }
}
