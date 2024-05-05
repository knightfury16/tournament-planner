using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Infrastructure.DataContext;

namespace TournamentPlanner.Infrastructure
{
    public class Repository<T, TResult> : IRepository<T, TResult> where T : class where TResult : T
    {
        private readonly TournamentPlannerDataContext _dataContext;
        public Repository(TournamentPlannerDataContext dataContext)
        {
            _dataContext = dataContext;

        }
        public async Task<TResult> AddAsync(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            _dataContext.Set<T>().Add(obj);
            await Task.CompletedTask;

            return (TResult)obj;
        }

        public async Task<IEnumerable<TResult>> GetAllAsync()
        {
            return (IEnumerable<TResult>)await _dataContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<TResult>> GetAllAsync(Func<T, bool> filter)
        {
            if (filter == null)
            {
                return await GetAllAsync();
            }
            var query = _dataContext.Set<T>().Where(filter);

            return (IEnumerable<TResult>)query.ToList();
        }

        public async Task<IEnumerable<TResult>> GetAllAsync(string[] includeProperties)
        {
            if (includeProperties is null || includeProperties.Length == 0)
            {
                return await GetAllAsync();
            }
            var query = _dataContext.Set<T>();
            foreach (var property in includeProperties)
            {
                query = (DbSet<T>)query.Include(p => p.GetType().GetProperty(property));
            }
            return (IEnumerable<TResult>)await query.ToListAsync();
        }

        public async Task<IEnumerable<TResult>> GetAllAsync(Func<T, bool> filter, string[] includeProperties)
        {
            var query = _dataContext.Set<T>();

            if (filter != null)
            {
                query = (DbSet<T>)query.Where(filter);
            }

            if (includeProperties != null && includeProperties.Length > 0)
            {
                foreach (var property in includeProperties)
                {
                    query = (DbSet<T>)query.Include(p => p.GetType().GetProperty(property));
                }
            }
            return (IEnumerable<TResult>)await query.ToListAsync();
        }

        public async Task<TResult?> GetByIdAsync(int id)
        {
            return (TResult?)await _dataContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<TResult>?> GetByNameAsync(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await GetAllAsync();
            }
            var query = _dataContext.Set<T>().Where(e => MatchNameProperty(e, name));

            return (IEnumerable<TResult>?)await query.ToListAsync();
        }

        private bool MatchNameProperty(T e, string name)
        {
            var nameProperty = e.GetType().GetProperty("Name");

            if (nameProperty != null)
            {

                var nameValue = nameProperty.GetValue(e)?.ToString();
                if (nameValue != null)
                {
                    return nameValue.ToLower().Equals(name.ToLower());
                }
            }
            return false;
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task<TResult> UpdateAsync(T obj)
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
            return (TResult)obj;
        }

        public Task<TResult> UpdateByIdAsync(int id, T obj)
        {
            throw new NotImplementedException();
        }
    }
}