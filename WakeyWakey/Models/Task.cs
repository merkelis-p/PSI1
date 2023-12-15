using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WakeyWakey.Enums;
using TaskStatus = WakeyWakey.Enums.TaskStatus;

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

        // ParentId to refer to the parent task if this is a subtask
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Task? ParentTask { get; set; }

        // Collection of subtasks if this task is a parent
        public virtual ICollection<Task>? Subtasks { get; set; }

        // SubjectId to link this task to a subject
        public int? SubjectId { get; set; }

        // Navigation property to Subject
        public virtual Subject? Subject { get; set; }

        // Properties for task details
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? EstimatedDuration { get; set; }
        public int? OverallDuration { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public int? Score { get; set; }
        public int? ScoreWeight { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Incompleted;

        // This property is not required in the database model and is used for form submission
        [NotMapped]
        public string? SubjectOrTaskId { get; set; }

        // Method to validate XOR logic for Subject and Task assignment
        public bool IsValidAssignment()
        {
            return (SubjectId == null) != (ParentId == null);
        }
    }
}