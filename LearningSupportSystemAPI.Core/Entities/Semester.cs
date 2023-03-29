namespace LearningSupportSystemAPI.Core.Entities
{
    public class Semester : BaseEntity
    {
        public SemesterType Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ExchangeRate { get; set; }

        public virtual ICollection<Class> Classes { get; set; } = new HashSet<Class>();
    }

    public enum SemesterType
    {
        First,
        Second,
        Summer
    }
}
