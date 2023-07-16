namespace LearningSupportSystemAPI;

public interface IGenerateIdService
{
    string GenerateUserIdCard();
    string GenerateStudentIdCard(Student student);
    string GenerateLecturerIdCard(Lecturer lecturer);
    string GenerateCourseCode(Course course);
    string GenerateClassCode(Class cla);
}
