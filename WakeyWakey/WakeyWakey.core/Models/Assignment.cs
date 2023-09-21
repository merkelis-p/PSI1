using System;
namespace WakeyWakey.WakeyWakey.core.Models
{
	public class Assignment
	{
		public DateTime DeadlineDate { get; set; }

		public int AssignmentID { get; set; }

		public int SubjectID { get; set; }

		public int ParentID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime EstimatedDuration { get; set; }

		public DateTime OverallDuration { get; set; }

	}
}

