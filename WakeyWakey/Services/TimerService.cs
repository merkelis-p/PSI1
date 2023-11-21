using Microsoft.AspNetCore.Http.HttpResults;
using WakeyWakey.Models;
namespace WakeyWakey.Services
{
    public class TimerService
    {
        private readonly IApiService<TimeRecorder> _timeTracker;
        public TimerService(IApiService<TimeRecorder> timeRecorder) {
            _timeTracker = timeRecorder;
        }

        public async void StartTimer(int id, TimeRecorder Time) // return type, probalby status code ok()?
        {
            ;
        }
    }
}
