
namespace TournamentPlanner.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T obj);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Func<T, bool> filter);
        Task<IEnumerable<T>> GetAllAsync(IEnumerable<Func<T, bool>> filters);

        // Any non-existent properties will be silently ignored by EF Core when building the query.
        Task<IEnumerable<T>> GetAllAsync(string[] includeProperties);

        Task<IEnumerable<T>> GetAllAsync(Func<T, bool> filter, string[] includeProperties);
        Task<IEnumerable<T>> GetAllAsync(IEnumerable<Func<T, bool>> filters, string[] includeProperties);

        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetByNameAsync(string? name);
        Task<T> UpdateAsync(T obj);
        Task<T> UpdateByIdAsync(int id, T obj);
        Task SaveAsync();
    }
}
