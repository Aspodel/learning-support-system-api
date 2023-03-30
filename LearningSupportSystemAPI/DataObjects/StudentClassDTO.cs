namespace LearningSupportSystemAPI.DataObjects
{
    public class StudentClassDTO
    {
        public string StudentId { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public int? GroupId { get; set; }
    }
}
