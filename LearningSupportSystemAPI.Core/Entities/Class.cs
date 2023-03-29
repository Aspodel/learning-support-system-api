using LearningSupportSystemAPI.Core.Entities.JoinTables;

namespace LearningSupportSystemAPI.Core.Entities
{
    public class Class : BaseEntity
    {
        public string ClassCode { get; set; } = string.Empty;
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public DayOfWeek Day { get; set; }
        public int Slot { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public string? LecturerId { get; set; }
        public Lecturer? Lecturer { get; set; }

        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public int SemesterId { get; set; }
        public Semester? Semester { get; set; }

        public virtual ICollection<StudentClass> Students { get; set; } = new HashSet<StudentClass>();
        public virtual ICollection<GradeColumn> GradeColumns { get; set; } = new HashSet<GradeColumn>();
    }
}
