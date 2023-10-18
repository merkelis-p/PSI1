using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WakeyWakey.Models
{
    public class Course
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        [Required(ErrorMessage = "Please fill the name area")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public Boolean CourseStatus { get; set; }
    }
}