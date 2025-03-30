using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stucation.Models
{
    public class StudentDto
    {
        public string studentId { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string surname { get; set; }
        public string dateCreated { get; set; }
        public string courseId { get; set; }
        public string gender { get; set; }
        public string mobileNumber { get; set; }
        public string email { get; set; }
        public string dob { get; set; }
        public string address { get; set; }
        public string status { get; set; }
    }
}