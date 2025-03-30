using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stucation.Models
{
    public class AssessmentDto
    {
        public string assessmentId { get; set; }
        public string assessmentTitle { get; set; }
        public string assessmentDetail { get; set; }
        public int assessmentTotalScore { get; set; }
        public string moduleId { get; set; }
        public string status { get; set; }
        public string dateCreated { get; set; }
    }
}