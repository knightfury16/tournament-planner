using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Infrastructure.DataContext;
using TournamentPlanner.Domain.Entities;
using System.Reflection.Metadata.Ecma335;

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
            return (IEnumerable<TResult>)await query.ToListAsync();
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

        public async Task<IEnumerable<TResult>> GetAllAsync(Func<T, bool> filter, string[] includeProperties)
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

            if (filter != null)
            {
                return (IEnumerable<TResult>)query.Where(filter).ToList();
            }

            return (IEnumerable<TResult>)await query.ToListAsync();
        }

        public async Task<IEnumerable<TResult>> GetAllAsync(IEnumerable<Func<T, bool>> filters)
        {
            if(filters == null)
            {
                return await GetAllAsync();
            }

            var query = _dataContext.Set<T>();

            foreach (var filter in filters)
            {
                query = (DbSet<T>)query.Where(filter);
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

            Expression<Func<T, bool>> lamdaPredicateExpression = BuildNameMatchingExpression<T>(name);

            var query = _dataContext.Set<T>().Where(lamdaPredicateExpression);

            return (IEnumerable<TResult>?)await query.ToListAsync();
        }

        public static Expression<Func<T, bool>> BuildNameMatchingExpression<T>(string name)
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

            var nameConstant = Expression.Constant(name); // No case-insensitive conversion
            var comparison = Expression.Equal(nameProperty, nameConstant);

            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
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