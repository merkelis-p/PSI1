using System.ComponentModel.DataAnnotations;
using WakeyWakey.Enums;

namespace WakeyWakey.Models
{
    public class Subject
    {

        public Subject()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddHours(1);
            Status = CourseStatus.NotStarted;
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(5000)]
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        public CourseStatus Status { get; set; }
        public int? Score { get; set; }
        public int? ScoreWeight { get; set; }
        public int Progress { get; set; }

        public int CourseId { get; set; }
        
        public IEnumerable<Task>? Tasks { get; set; }
    }
}