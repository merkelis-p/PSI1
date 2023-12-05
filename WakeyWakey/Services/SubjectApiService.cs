using System.Text.Json;
using WakeyWakey.Models;

namespace WakeyWakey.Services;

public class SubjectApiService : ApiService<Subject>, ISubjectApiService
{
    
    public SubjectApiService(IConfiguration configuration, ILogger<SubjectApiService> logger) : base(configuration, logger)
    {
        
    }
    
    public async Task<IEnumerable<Subject>> GetSubjectsByCourseIdAsync(int courseId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Subject/GetSubjectsByCourse/{courseId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<Subject>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else {
                return Enumerable.Empty<Subject>(); // return an empty list
            }
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<Subject>(); // return an empty list
        }
    }

    
}