using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.DataObjects;

namespace LearningSupportSystemAPI
{
    public class AdminDTO : UserDTO
    {
        public AdminType Type { get; set; }
    }

    public class CreateAdminDTO : CreateUserDTO
    {
        public AdminType Type { get; set; }
    }
}
