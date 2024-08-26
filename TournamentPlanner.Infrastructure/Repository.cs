using System.Linq.Expressions;
using System.Data;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.DataModeling;

namespace TournamentPlanner.Infrastructure
{
    /// <summary>
    /// Generic repository class for performing CRUD operations on entities of type T.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TournamentPlannerDataContext _dataContext;
        public Repository(TournamentPlannerDataContext dataContext)
        {
            _dataContext = dataContext;

        }
        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _dataContext.Set<T>().AddAsync(entity);

            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dataContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                return await GetAllAsync();
            }
            var query = _dataContext.Set<T>().Where(filter);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(string[] includeProperties)
        {
            if (includeProperties is null || includeProperties.Length == 0)
            {
                return await GetAllAsync();
            }
            var query = _dataContext.Set<T>().AsQueryable();

            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter, string[] includeProperties)
        {
            var query = _dataContext.Set<T>().AsQueryable();

            if (includeProperties != null && includeProperties.Length > 0)
            {
                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }

            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();

        }

        public async Task<IEnumerable<T>> GetAllAsync(IEnumerable<Expression<Func<T, bool>>> filters, string[] includeProperties)
        {
            var query = _dataContext.Set<T>().AsQueryable();

            if (includeProperties != null && includeProperties.Length > 0)
            {
                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }
            }

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(IEnumerable<Expression<Func<T, bool>>> filters)
        {
            if (filters == null)
            {
                return await GetAllAsync();
            }

            var query = _dataContext.Set<T>().AsQueryable();

            if(filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public Task<IEnumerable<T>> GetByNameAsync(string? name)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            // Check if the entity is already tracked (optional)
            if (!_dataContext.ChangeTracker.Entries<T>().Any(e => e.Entity.GetType().GetProperty("Id")?.GetValue(e) == obj.GetType().GetProperty("Id")?.GetValue(obj)))
            {
                _dataContext.Set<T>().Attach(obj); // Attach only if not tracked
            }
            else
            {
                _dataContext.Entry(obj).State = EntityState.Modified; // Set state explicitly (optional)
            }
            await Task.CompletedTask;
            return obj;
        }

        public Task<T> UpdateByIdAsync(int id, T obj)
        {
            throw new NotImplementedException();
        }

    }
}