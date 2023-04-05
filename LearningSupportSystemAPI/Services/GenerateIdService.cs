using LearningSupportSystemAPI.Core.Entities;
using LearningSupportSystemAPI.Repository;
using System.Text;

namespace LearningSupportSystemAPI.Services
{
    public class GenerateIdService : IGenerateIdService
    {
        #region [Fields]
        private readonly UserManager _userManager;
        #endregion

        #region [Ctor]
        public GenerateIdService(UserManager userManager)
        {
            _userManager = userManager;
        }
        #endregion

        #region [Methods]
        public string GenerateStudentIdCard(Student student)
        {
            var prefix = student.Department!.ShortName + student.Major!.ShortName + "IU" + student.StartYear.ToString()![^2..]; // create prefix from parameters
            var suffix = GenerateSuffix(prefix, _userManager.FindAllStudents(x => x.IdCard.StartsWith(prefix))); // use private method to generate suffix
            return prefix + suffix; // return concatenated string
        }

        public string GenerateUserId()
        {
            var prefix = "IU";
            var suffix = GenerateSuffix(prefix, _userManager.FindAll(x => x.IdCard.StartsWith(prefix))); // use private method to generate suffix
            return prefix + suffix; // return concatenated string
        }

        // private method to generate a suffix from a prefix and a collection of users
        private static string GenerateSuffix(string prefix, IEnumerable<User> users)
        {
            var suffix = users.Max(u => u.IdCard.Substring(prefix.Length)); // find the maximum existing suffix

            if (suffix == null) // no existing IdCard with this prefix
            {
                suffix = "00001"; // start from 00001
            }
            else
            {
                suffix = (int.Parse(suffix) + 1).ToString("D5"); // increment by one and pad with zeros
            }
            return suffix; // return the generated suffix
        }
        #endregion
    }
}
