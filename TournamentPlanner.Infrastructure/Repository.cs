using System.Linq.Expressions;
using System.Data;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.DataModeling;

namespace TournamentPlanner.Infrastructure
{
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
                var propertyInfo = typeof(T).GetProperty(property);
                if (propertyInfo != null)
                {
                    Expression<Func<T, object>> lambdaExpression = BuildTheIncludeExpression(property);

                    query = query.Include(lambdaExpression);
                }
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
                    Expression<Func<T, object>> includeLambdaExpression = BuildTheIncludeExpression(property);

                    query = query.Include(includeLambdaExpression);
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
                    Expression<Func<T, object>> lambdaExpression = BuildTheIncludeExpression(property);

                    query = query.Include(lambdaExpression);
                }
            }

            if (filters != null && filters.Count() > 1)
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

            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetByNameAsync(string? name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                return await GetAllAsync();
            }

            Expression<Func<T, bool>> lamdaPredicateExpression = BuildNameMatchingExpression(name);

            var query = _dataContext.Set<T>().Where(lamdaPredicateExpression);

            return await query.ToListAsync();
        }

        public static Expression<Func<T, bool>> BuildNameMatchingExpression(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }

            //TODO: Add case insensitivity
            // var nameLowered = Expression.Call(nameProperty, typeof(string).GetMethod("ToLowerInvariant", BindingFlags.Static | BindingFlags.Public)); // Safer method call
            var parameter = Expression.Parameter(typeof(T), "x");
            var nameProperty = Expression.Property(parameter, "Name");

            if (nameProperty == null)
            {
                throw new ArgumentException("No Name property to filter by on this object");
            }

            // Use Contains method (assuming Name is a string)
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!; // a string will always have Contains method
            var nameConstant = Expression.Constant(name); // No case-insensitive conversion
            
            var containsCall = Expression.Call(nameProperty, containsMethod, nameConstant);

            // var comparison = Expression.Equal(nameProperty, nameConstant);

            return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
        }

        //Naive I am
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

        private static Expression<Func<T, object>> BuildTheIncludeExpression(string property)
        {
            // Construct an expression for the include property
            // it select parameter as x of Object Player
            var parameter = Expression.Parameter(typeof(T), "x"); // x here is just a random name

            // Split the property string by dots to handle nested properties
            var propertyNames = property.Split('.');

            // for example x => x.Tournament
            //propertyExpression type Tournament
            //var propertyExpression = Expression.Property(parameter, property);

            Expression propertyExpression = parameter;

            // Iterate through each property name to build the nested property expression
            foreach (var propertyName in propertyNames)
            {
                propertyExpression = Expression.Property(propertyExpression, propertyName);
            }


            //TODO:: I dont understand this, need no know more about Expression
            Expression<Func<T, object>> lambdaExpression = Expression.Lambda<Func<T, object>>(propertyExpression, parameter);
            return lambdaExpression;
        }
    }
}