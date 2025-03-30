using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Stucation.Models
{
    public class LoginDto
    {
        [Required]
        public string username { get; set; }
    }
}