using System.Text.Json;
using WakeyWakey.Models;

namespace WakeyWakey.Services;

public class CourseApiService : ApiService<Course>, ICourseApiService
{
    
    private readonly ILogger<CourseApiService> _logger;
    
    public CourseApiService(IConfiguration configuration, ILogger<CourseApiService> logger) : base(configuration, logger)
    {
        _logger = logger;
        
    }
    
    
    // Get courses by user id
    public async Task<IEnumerable<Course>> GetCoursesByUserIdAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Course/GetCoursesByUserId/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<Course>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else {
                return Enumerable.Empty<Course>(); // return an empty list
            }
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<Course>(); // return an empty list
        }
    }


    public async Task<IEnumerable<Course>> GetAllHierarchyAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Course/GetAllHierarchy/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<Course>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                _logger.LogError(
                    $"Failed to get course hierarchy. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
                return Enumerable.Empty<Course>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in GetAllHierarchyAsync: {ex.Message}");
            return Enumerable.Empty<Course>();
        }
    }


}