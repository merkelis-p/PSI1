
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WakeyWakey.Services;

public interface IApiService<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<bool> UpdateAsync(int id, T entity);
    Task<bool> DeleteAsync(int id);
}
