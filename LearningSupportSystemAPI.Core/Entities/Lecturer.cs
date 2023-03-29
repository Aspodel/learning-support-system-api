namespace LearningSupportSystemAPI.Core.Entities
{
    public class Lecturer : User
    {
        public string? Description { get; set; }
        public int? Salary { get; set; }

        public virtual ICollection<Class> Classes { get; set; } = new HashSet<Class>();
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
