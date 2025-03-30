using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stucation.Models
{
    public class StudentAssessmentDto
    {
        public string studentAssessmentId { get; set; }
        public string studentId { get; set; }
        public string AssessmentId { get; set; }
        public string score { get; set; }
        public string status { get; set; }
        public string dateCreated { get; set; }
    }
}