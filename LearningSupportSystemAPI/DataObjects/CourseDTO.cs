namespace LearningSupportSystemAPI;

public class CourseDTO : BaseDTO
{
    public string CourseCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public bool? GpaCalculated { get; set; } = true;

    public int DepartmentId { get; set; }
    public ICollection<MajorDTO>? Majors { get; set; }
    public ICollection<ClassDTO> Classes { get; set; } = new HashSet<ClassDTO>();
    public ICollection<CoursePrerequisiteDTO>? PrerequisiteFor { get; set; }
    public ICollection<CoursePrerequisiteDTO>? Prerequisites { get; set; }
}

public class CreateCourseDTO
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? Credits { get; set; }
    public bool? GpaCalculated { get; set; } = true;

    public int DepartmentId { get; set; }
    public ICollection<int>? PrerequisiteFor { get; set; }
    public ICollection<int>? Prerequisites { get; set; }
}

public class ScheduleRecommendationDTO
{
    public ICollection<int> Courses { get; set; } = new HashSet<int>();
    public Constraint Constraint { get; set; } = new Constraint();
}
