using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentPlanner.Application.Common.Interfaces
{
    public interface IRepository<T, TResult> where T : class
    {
        Task<TResult> AddAsync(T obj);
        Task<IEnumerable<TResult>> GetAllAsync();
        Task<TResult> GetByIdAsync(int id);
        Task<IEnumerable<TResult>> GetByNameAsync(string? name);
        Task<TResult> UpdateAsync(T obj);
        Task<TResult> UpdateByIdAsync(int id, T obj);
    }
}
