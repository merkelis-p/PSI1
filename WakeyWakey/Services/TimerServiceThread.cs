using NodaTime;
using System;
using System.Threading;
using System.Timers;
using WakeyWakey.Models;

namespace WakeyWakey.Services
{
    public class TimeServiceThread
    {
        private readonly IApiService<TimeRecorder> _timeTracker;

        public TimeServiceThread(IApiService<TimeRecorder> timeTracker)
        {
            _timeTracker = timeTracker;
        }

        public void StartTimer(int id, DateTime startTime)
        {
            var timeRecorder = new TimeRecorder
            {
                RecorderId = id,
                StartTimeStamp = startTime
            };
            Thread timerThread = new Thread(() =>
            {
                while (true)
                {
                    var elapsedTime = DateTime.Now - startTime;
                    timeRecorder.Duration = elapsedTime;
                    if (elapsedTime.TotalMinutes % 5 == 0)
                    {
                        _timeTracker.UpdateAsync(id, timeRecorder);
                    }
                    Thread.Sleep(1000);
                }
            });
            timerThread.Start();
        }
    }
}
