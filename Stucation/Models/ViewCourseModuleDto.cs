using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stucation.Models
{
    public class ViewCourseModuleDto
    {
        public CourseModuleDto courseModule { get; set; }
        public CourseDto course { get; set; }
        public ModuleDto module { get; set; }
    }
}