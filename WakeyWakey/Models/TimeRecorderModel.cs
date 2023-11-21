using System;

namespace WakeyWakey.Models;
public class TimeRecorder
{
    public int RecorderId { get; set; }
    public DateTime StartTimeStamp { get; set; }
	public DateTime EndTimeStamp { get; set; }
	public TimeSpan BreakDuration { get; set; }
	public TimeSpan FocusDuration { get; set; }
	public TimeSpan Duration { get; set; }
	public int BreakFreaquency { get; set; } = 1;
	public string catagory { get; set; }
    public string Note { get; set; } = string.Empty;
}

