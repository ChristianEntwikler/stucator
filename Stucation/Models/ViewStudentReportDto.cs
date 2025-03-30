using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stucation.Models
{
    public class ViewStudentReportDto
    {
        public ViewStudentAssessmentDto viewStudentAssessment { get; set; }
        public float totalScore { get; set; }
        public string grade { get; set; }
    }
}