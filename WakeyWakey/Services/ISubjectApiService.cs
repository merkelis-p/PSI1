using WakeyWakey.Models;

namespace WakeyWakey.Services;

public interface ISubjectApiService : IApiService<Subject>
{

    public Task<IEnumerable<Subject>> GetSubjectsByCourseIdAsync(int courseId);


}