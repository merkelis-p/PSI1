using WakeyWakey.Models;

namespace WakeyWakey.Services;

public interface IEventApiService : IApiService<Event>
{
    public Task<IEnumerable<Event>> GetEventsByUserIdAsync(int userId);
}