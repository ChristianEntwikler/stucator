using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stucation.Models
{
    public class ViewStudentModuleDto
    {
        public StudentModuleDto studentModule { get; set; }
        public StudentDto student { get; set; }
        public ModuleDto module { get; set; }
    }
}