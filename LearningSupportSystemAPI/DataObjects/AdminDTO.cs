namespace LearningSupportSystemAPI;

public class AdminDTO : UserDTO
{
    public AdminType Type { get; set; }
}

public class CreateAdminDTO : CreateUserDTO
{
    public AdminType Type { get; set; }
}
