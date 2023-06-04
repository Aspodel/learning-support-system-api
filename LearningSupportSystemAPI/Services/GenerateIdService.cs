using LearningSupportSystemAPI.Core.Database;
using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI;

public class GenerateIdService : IGenerateIdService
{
    #region [Fields]
    private readonly ApplicationDbContext _context;
    #endregion

    #region [Ctor]
    public GenerateIdService(ApplicationDbContext context)
    {
        _context = context;
    }
    #endregion

    #region [Methods]
    private string GenerateSuffix<T>(string prefix, Func<T, string> idSelector) where T : class
    {
        // var suffix = _context.Set<T>().ToList().Max(x => idSelector(x).Substring(prefix.Length)); // find the maximum existing suffix
        // suffix = suffix == null ? "001" : (int.Parse(suffix) + 1).ToString("D3"); // increment by one and pad with zeros

        // return suffix; // return the generated suffix
        var existingSuffixes = _context.Set<T>().Select(x => idSelector(x)).ToList();

        var suffix = 1;
        while (existingSuffixes.Contains($"{prefix}{suffix:D3}"))
        {
            suffix++;
        }

        return suffix.ToString("D3");
    }

    public string GenerateUserIdCard()
    {
        var prefix = "User";
        var suffix = GenerateSuffix<User>(prefix, u => u.IdCard); // use private method to generate suffix
        return prefix + suffix; // return concatenated string
    }

    public string GenerateStudentIdCard(Student student)
    {
        var department = student.Department!.ShortName; // get department short name
        var major = student.Major?.ShortName ?? department;
        var prefix = $"{department}{major}{student.StartYear % 100}"; // create prefix from parameters
        var suffix = GenerateSuffix<Student>(prefix, s => s.IdCard); // use private method to generate suffix
        return prefix + suffix; // return concatenated string
    }

    public string GenerateLecturerIdCard(Lecturer lecturer)
    {
        var department = lecturer.Department!.ShortName; // get department short name
        var prefix = $"{department}"; // create prefix from parameters
        var suffix = GenerateSuffix<Lecturer>(prefix, l => l.IdCard); // use private method to generate suffix
        return prefix + suffix; // return concatenated string
    }

    public string GenerateCourseCode(Course course)
    {
        var department = course.Department!.ShortName; // get department short name
        var prefix = $"{department}"; // create prefix from parameters
        var suffix = GenerateSuffix<Course>(prefix, c => c.CourseCode); // use private method to generate suffix
        return prefix + suffix; // return concatenated string
    }

    public string GenerateClassCode(Class cla)
    {
        var course = cla.Course!.CourseCode; // get department short name
        var prefix = $"{course}"; // create prefix from parameters
        var suffix = GenerateSuffix<Class>(prefix, c => c.ClassCode); // use private method to generate suffix
        return prefix + suffix; // return concatenated string
    }
    #endregion
}
