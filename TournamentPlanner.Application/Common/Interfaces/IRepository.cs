using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentPlanner.Application.Common.Interfaces
{
    public interface IRepository<T, TResult> where T : class where TResult : T
    {
        Task<TResult> AddAsync(T obj);
        Task<IEnumerable<TResult>> GetAllAsync();
        Task<IEnumerable<TResult>> GetAllAsync(Func<T, bool> filter);
        Task<IEnumerable<TResult>> GetAllAsync(IEnumerable<Func<T, bool>> filters);

        // Any non-existent properties will be silently ignored by EF Core when building the query.
        Task<IEnumerable<TResult>> GetAllAsync(string[] includeProperties);

        Task<IEnumerable<TResult>> GetAllAsync(Func<T, bool> filter, string[] includeProperties);
        Task<TResult?> GetByIdAsync(int id);
        Task<IEnumerable<TResult>?> GetByNameAsync(string? name);
        Task<TResult> UpdateAsync(T obj);
        Task<TResult> UpdateByIdAsync(int id, T obj);
        Task SaveAsync();
    }
}
