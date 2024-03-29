﻿namespace LearningSupportSystemAPI;

public class GradeColumn : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool? IsPublished { get; set; } = false;
    public int Percentage { get; set; }
    public int? Order { get; set; }

    public int ClassId { get; set; }
    public Class? Class { get; set; }

    public ICollection<Grade>? Grades { get; set; }
}
