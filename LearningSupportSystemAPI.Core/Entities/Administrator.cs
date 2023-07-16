namespace LearningSupportSystemAPI;

public class Administrator : User
{
    public AdminType Type { get; set; } = AdminType.System;
}

public enum AdminType
{
    System,
    School,
    Department,
}
