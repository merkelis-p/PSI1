using System;

namespace WakeyWakey.Models
{
    public class CourseModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public Boolean CourseStatus { get; set; }
    }
}