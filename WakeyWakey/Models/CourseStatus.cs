using System.ComponentModel;

namespace WakeyWakey.Models;

public enum CourseStatus
{
    
    [Description("Pending")]
    Pending = 0,
    [Description("Not Started")]
    NotStarted = 1,
    [Description("In Progress")]
    Active = 2,
    [Description("Completed")]
    Completed = 3,
    [Description("Cancelled")]
    Cancelled = 4
}
