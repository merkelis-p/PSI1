using System.Text.Json;
using WakeyWakey.Models;
using Task = System.Threading.Tasks.Task;

namespace WakeyWakey.Services;


public class TaskApiService : ApiService<Models.Task>
{
    
    public TaskApiService(IConfiguration configuration, ILogger<TaskApiService> logger) : base(configuration, logger)
    {
        
    }
    
    
    // Get Tasks by user id
    public async Task<IEnumerable<Models.Task>> GetTasksByUserIdAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Task/GetByUserId/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<Models.Task>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else {
                return Enumerable.Empty<Models.Task>(); // return an empty list
            }
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<Models.Task>(); // return an empty list
        }
    }
    
    // Get Tasks by subject id
    public async Task<IEnumerable<Models.Task>> GetTasksBySubjectIdAsync(int subjectId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Task/GetBySubjectId/{subjectId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<Models.Task>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else {
                return Enumerable.Empty<Models.Task>(); // return an empty list
            }
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<Models.Task>(); // return an empty list
        }
    }
    
    // Get Tasks by parent id
    public async Task<IEnumerable<Models.Task>> GetTasksByParentIdAsync(int parentId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Task/GetChildrenByParentId/{parentId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<Models.Task>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else {
                return Enumerable.Empty<Models.Task>(); // return an empty list
            }
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<Models.Task>(); // return an empty list
        }
    }
    
    // Get tasks with hierarchy by user id
    public async Task<IEnumerable<Models.Task>> GetTasksWithHierarchyByUserIdAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Task/GetTasksWithHierarchyByUserId/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<Models.Task>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else {
                return Enumerable.Empty<Models.Task>(); // return an empty list
            }
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<Models.Task>(); // return an empty list
        }
    }
    
    
    
    
}