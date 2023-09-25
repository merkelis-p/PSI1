using System;
namespace WakeyWakey.WakeyWakey.core.Models
{
    public class TimeRecorder
    {
        public DateTime StartTimeStamp { get; set; }

        public DateTime EndTimeStamp { get; set; }

        public string Note { get; set; }

        public TimeSpan Duration
        {
            get
            {
                return EndTimeStamp - StartTimeStamp;
            }
        }


    }
}

