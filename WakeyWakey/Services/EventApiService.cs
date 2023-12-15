using Newtonsoft.Json;
using WakeyWakey.Models;


namespace WakeyWakey.Services;

public class EventApiService : ApiService<Event>, IEventApiService
{
    
    public EventApiService(IConfiguration configuration, ILogger<EventApiService> logger) : base(configuration, logger)
    {
        
    }
    
    public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Event/GetByUserId/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Event>>(content);
            }
            else {
                return Enumerable.Empty<Event>();
            }
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<Event>();
        }
    }
    
}