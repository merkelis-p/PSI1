using Microsoft.AspNetCore.Http.HttpResults;
using WakeyWakey.Models;
namespace WakeyWakey.Services
{
    public class TimerService
    {
        private readonly IApiService<Record> _timeTracker;
        public TimerService(IApiService<Record> record) {
            _timeTracker = record;
        }

        public async void StartTimer(int id, Record Time) // return type, probalby status code ok()?
        {
            ;
        }
    }
}
