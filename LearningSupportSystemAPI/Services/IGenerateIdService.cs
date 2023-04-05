using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.Services
{
    public interface IGenerateIdService
    {
        string GenerateStudentIdCard(Student student);
        string GenerateUserId();
    }
}
