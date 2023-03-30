namespace LearningSupportSystemAPI.DataObjects
{
    public class GradeDTO : BaseDTO
    {
        public int Value { get; set; }

        public int GradeColumnId { get; set; }
        public string StudentId { get; set; } = string.Empty;
    }
}
