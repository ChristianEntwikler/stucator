using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stucation.Models
{
    public class CourseModuleDto
    {
        public string courseModuleId { get; set; }
        public string courseId { get; set; }
        public string moduleId { get; set; }
        public string priority { get; set; }
        public string status { get; set; }
        public string dateCreated { get; set; }
    }
}