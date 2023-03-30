using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.Core.Entities.JoinTables;

namespace LearningSupportSystemAPI.DataObjects
{
    public class ClassDTO : BaseDTO
    {
        public string ClassCode { get; set; } = string.Empty;
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public DayOfWeek Day { get; set; }
        public int Slot { get; set; }

        public int CourseId { get; set; }
        public string? LecturerId { get; set; }
        public int? RoomId { get; set; }
        public int SemesterId { get; set; }

        public virtual ICollection<StudentClass> Students { get; set; } = new HashSet<StudentClass>();
        public virtual ICollection<GradeColumn> GradeColumns { get; set; } = new HashSet<GradeColumn>();

    }
}
