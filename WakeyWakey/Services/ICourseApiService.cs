using WakeyWakey.Models;

namespace WakeyWakey.Services;

public interface ICourseApiService : IApiService<Course>
{
    Task<IEnumerable<Course>> GetCoursesByUserIdAsync(int userId);
    Task<IEnumerable<Course>> GetAllHierarchyAsync(int userId);
}