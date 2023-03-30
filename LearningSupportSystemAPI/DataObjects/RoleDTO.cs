using System.ComponentModel.DataAnnotations;

namespace LearningSupportSystemAPI.DataObjects
{
    public class RoleDTO : BaseDTO<string>
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
