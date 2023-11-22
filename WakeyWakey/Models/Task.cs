using System.ComponentModel.DataAnnotations.Schema;
using WakeyWakey.Enums;
namespace WakeyWakey.Models
{
    public partial class Task 
    {
        public Task()
        {
            Subtasks = new HashSet<Task>();
        }
        public int Id { get; set; }
        public TaskCategory Category { get; set; }
        public int? ParentId { get; set; }
        
        [ForeignKey("ParentId")]
        public virtual Task? ParentTask { get; set; }
        public virtual ICollection<Task>? Subtasks { get; set; }
        
        public int? SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? EstimatedDuration { get; set; }
        public int? OverallDuration { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public int? Score { get; set; }
        public int? ScoreWeight { get; set; }
        public required Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Incompleted;
        
        public IEnumerable<Task>? Tasks { get; set; }
        
        public string? SubjectOrTaskId { get; set; }



    }
}
