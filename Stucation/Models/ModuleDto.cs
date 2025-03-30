using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stucation.Models
{
    public class ModuleDto
    {
        public string moduleId { get; set; }
        public string moduleName { get; set; }
        public string moduleDetails { get; set; }
        public string term { get; set; }
        public string status { get; set; }
        public string dateCreated { get; set; }
    }
}