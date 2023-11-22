using NodaTime;
using System;
using System.Threading;
using System.Timers;
using WakeyWakey.Models;

namespace WakeyWakey.Services
{
    public class TimeServiceThread
    {
        private readonly IApiService<Record> _timeTracker;

        public TimeServiceThread(IApiService<Record> timeTracker)
        {
            _timeTracker = timeTracker;
        }

        public void StartTimer(int id, DateTime startTime)
        {
            var Record = new Record
            {
                RecorderId = id,
                StartTimeStamp = startTime
            };
            Thread timerThread = new Thread(() =>
            {
                while (true)
                {
                    var elapsedTime = DateTime.Now - startTime;
                    Record.Duration = elapsedTime;
                    if (elapsedTime.TotalMinutes % 5 == 0)
                    {
                        _timeTracker.UpdateAsync(id, Record);
                    }
                    Thread.Sleep(1000);
                }
            });
            timerThread.Start();
        }
    }
}
