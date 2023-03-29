namespace LearningSupportSystemAPI.Core.Entities
{
    public class Room : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public int? Type { get; set; }
        public string Building { get; set; } = string.Empty;
        public int? Seat { get; set; }

        public Department? Department { get; set; }

        public virtual ICollection<Class> Classes { get; set; } = new HashSet<Class>();
    }
}
