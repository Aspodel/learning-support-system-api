using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningSupportSystemAPI.Core.Entities.JoinTables
{
    public class StudentClass
    {
        public string StudentId { get; set; } = string.Empty;
        public Student? Student { get; set; }

        public int ClassId { get; set; } 
        public Class? Class { get; set; } 

        public int? GroupId { get; set; } 
        public Group? Group { get; set; } 
    }
}
