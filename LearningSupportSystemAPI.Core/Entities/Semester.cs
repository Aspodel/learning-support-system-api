namespace LearningSupportSystemAPI.Core.Entities
{
    public class Semester : BaseEntity
    {
        public SemesterType Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string AcademicYear { get; set; } = string.Empty;
        public int? ExchangeRate { get; set; }

        public ICollection<Class>? Classes { get; set; }
    }

    public enum SemesterType
    {
        Fall,
        Spring,
        Summer
    }
}
