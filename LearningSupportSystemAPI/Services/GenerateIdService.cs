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
        var maxSuffix = _context.Set<T>()
                            .Select(idSelector)
                            .Where(x => x.StartsWith(prefix))
                            .Select(x => x.Substring(prefix.Length))
                            .ToList()
                            .Max(); // find the maximum existing suffix

        var newSuffix = maxSuffix == null ? "001" : (int.Parse(maxSuffix) + 1).ToString("D3"); // increment by one and pad with zeros
        return newSuffix;
    }

    public string GenerateUserIdCard()
    {
        var prefix = "User";
        var suffix = GenerateSuffix<User>(prefix, u => u.IdCard);
        return prefix + suffix;
    }

    public string GenerateStudentIdCard(Student student)
    {
        var department = student.Department!.ShortName;
        var major = student.Major?.ShortName ?? department;
        var prefix = $"{department}{major}{student.StartYear % 100}"; // create prefix from parameters
        var suffix = GenerateSuffix<Student>(prefix, s => s.IdCard);
        return prefix + suffix;
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
