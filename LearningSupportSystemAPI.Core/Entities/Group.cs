using LearningSupportSystemAPI.Core.Entities.JoinTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningSupportSystemAPI.Core.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;

        public virtual ICollection<StudentClass> Students { get; set; } = new HashSet<StudentClass>();
    }
}
