using LearningSupportSystemAPI.Core.Entities.JoinTables;

namespace LearningSupportSystemAPI.Core.Entities
{
    public class Student : User
    {
        public DateTime? StartYear { get; set; } = DateTime.UtcNow;

        public int MajorId { get; set; }
        public Major? Major { get; set; }

        public string SupervisorId { get; set; } = string.Empty;
        public Lecturer? Supervisor { get; set; }

        public virtual ICollection<StudentClass> RegisteredClasses { get; set; } = new HashSet<StudentClass>();
        public virtual ICollection<Grade> Grades { get; set; } = new HashSet<Grade>();
    }
}
