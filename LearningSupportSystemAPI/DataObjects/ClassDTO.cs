namespace LearningSupportSystemAPI;

public class ClassDTO : BaseDTO
{
    public string ClassCode { get; set; } = string.Empty;
    public int StartTime { get; set; }
    public int EndTime { get; set; }
    public DayOfWeek Day { get; set; }
    public int Slot { get; set; }

    public int CourseId { get; set; }
    public string LecturerId { get; set; } = string.Empty;
    public int? RoomId { get; set; }
    public int? SemesterId { get; set; }

    public CourseDTO? Course { get; set; }
    public ICollection<StudentClassDTO>? Students { get; set; }
    public ICollection<GradeColumnDTO>? GradeColumns { get; set; }
}

public class CreateClassDTO
{
    public int StartTime { get; set; }
    public int EndTime { get; set; }
    public DayOfWeek Day { get; set; }
    public int Slot { get; set; }

    public int CourseId { get; set; }
    public string LecturerId { get; set; } = string.Empty;
    public int? RoomId { get; set; }
    public int? SemesterId { get; set; }
}

public class CreateClassExcelDTO
{
    public int StartTime { get; set; }
    public int EndTime { get; set; }
    public DayOfWeek Day { get; set; }
    public int Slot { get; set; }

    public string CourseCode { get; set; } = string.Empty;
    public string LecturerIdCard { get; set; } = string.Empty;
}
