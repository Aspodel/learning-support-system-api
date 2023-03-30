using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.DataObjects
{
    public class SemesterDTO : BaseDTO
    {
        public SemesterType Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ExchangeRate { get; set; }

        public virtual ICollection<Class> Classes { get; set; } = new HashSet<Class>();

    }
}
