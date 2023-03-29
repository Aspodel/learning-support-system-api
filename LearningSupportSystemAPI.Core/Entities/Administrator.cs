using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningSupportSystemAPI.Core.Entities
{
    public class Administrator : User
    {
        public int Level { get; set; }
    }
}
