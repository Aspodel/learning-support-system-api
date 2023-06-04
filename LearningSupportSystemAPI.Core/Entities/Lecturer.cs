namespace LearningSupportSystemAPI.Core.Entities
{
    public class Lecturer : User
    {
        public string? Description { get; set; }
        public int? Salary { get; set; }

        public ICollection<Class>? Classes { get; set; }
        public ICollection<Student>? Students { get; set; }
    }
}
