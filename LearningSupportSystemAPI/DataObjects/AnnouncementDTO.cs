namespace LearningSupportSystemAPI.DataObjects
{
    public class AnnouncementDTO : BaseDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public string SenderId { get; set; } = string.Empty;
    }
}
