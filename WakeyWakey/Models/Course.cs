using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WakeyWakey.Enums;

namespace WakeyWakey.Models
{
    public class Course
    {

        public Course()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddHours(1);
            Status = CourseStatus.Pending;
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(5000)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public CourseStatus Status { get; set; }
        public int? Score { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public List<Subject>? Subjects { get; set; }

    }
}