using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stucation.Models
{
    public class CourseDto
    {
        public string courseId { get; set; }
        public string courseName { get; set; }
        public string courseDetails { get; set; }
        public string coursePeriod { get; set; }
        public int maximumStudents { get; set; }
        public string status { get; set; }
        public string dateCreated { get; set; }
        public ImageDto imageData { get; set; }
    }
}