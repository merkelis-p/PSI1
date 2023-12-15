using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace WakeyWakey.Models;
public class Record
{
    public int RecorderId { get; set; }
    public DateTime StartTimeStamp { get; set; }
	public DateTime EndTimeStamp { get; set; }
	public TimeSpan BreakDuration { get; set; }
	public TimeSpan FocusDuration { get; set; }
	public TimeSpan Duration { get; set; }
	public int BreakFrequency { get; set; } = 1;
	public string catagory { get; set; }
    public string Note { get; set; } = string.Empty;

    [BindNever]
    public int FocusHours { get; set; }

    [BindNever]
    public int FocusMinutes { get; set; }

    [BindNever]
    public int BreakHours { get; set; }

    [BindNever]
    public int BreakMinutes { get; set; }
}

