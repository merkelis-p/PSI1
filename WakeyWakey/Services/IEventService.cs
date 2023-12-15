using System.Collections.Generic;
using System.Threading.Tasks;

namespace WakeyWakey.Services;

public interface IEntityService<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<bool> UpdateAsync(int id, T entity);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<T>> GetDayEventsAsync(DateTime date);
    Task<IEnumerable<T>> GetDateRangeEventsAsync(DateTime startDate, DateTime endDate);

}

