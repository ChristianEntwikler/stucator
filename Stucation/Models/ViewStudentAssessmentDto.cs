using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stucation.Models
{
    public class ViewStudentAssessmentDto
    {
        public StudentAssessmentDto studentAssessment { get; set; }
        public StudentDto student { get; set; }
        public AssessmentDto assessment { get; set; }
        public ModuleDto module { get; set; }
    }
}