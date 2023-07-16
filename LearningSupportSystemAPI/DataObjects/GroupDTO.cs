namespace LearningSupportSystemAPI;

public class GroupDTO : BaseDTO
{
    public string Name { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;

    public ICollection<StudentClassDTO>? Students { get; set; }
}

public class CreateGroupDTO
{
    public string Name { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;

    public ICollection<string>? Students { get; set; }
}

