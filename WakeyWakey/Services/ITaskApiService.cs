using WakeyWakey.Models;
namespace WakeyWakey.Services;

public interface ITaskApiService : IApiService<Models.Task>
{
    public Task<IEnumerable<Models.Task>> GetTasksByUserIdAsync(int userId);
    public Task<IEnumerable<Models.Task>> GetTasksBySubjectIdAsync(int subjectId);
    public Task<IEnumerable<Models.Task>> GetTasksByParentIdAsync(int parentId);
    public Task<IEnumerable<Models.Task>> GetTasksWithHierarchyByUserIdAsync(int userId);
}