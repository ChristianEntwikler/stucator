using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stucation.Models
{
    public class StudentModuleDto
    {
        public string studentModuleId { get; set; }
        public string studentId { get; set; }
        public string moduleId { get; set; }
        public string status { get; set; }
        public string dateCreated { get; set; }
    }
}