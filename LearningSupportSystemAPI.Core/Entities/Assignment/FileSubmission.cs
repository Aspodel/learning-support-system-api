namespace LearningSupportSystemAPI;

public class FileSubmission : Submission
{
    public string FileName { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
}