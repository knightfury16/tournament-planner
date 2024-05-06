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
        //TODO:: Chnage the access to private later
        public readonly TournamentPlannerDataContext _dataContext;
        public Repository(TournamentPlannerDataContext dataContext)
        {
            _dataContext = dataContext;

        }
        public async Task<TResult> AddAsync(T obj)
        {
            var state = _dataContext.Entry(obj).State;
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            _dataContext.Set<T>().Add(obj);

            state = _dataContext.Entry(obj).State;

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
            /* foreach (var property in includeProperties)
             {
                 query = query.Include(p =>
                 {
                     var check = p.GetType().GetProperty(property);
                     if(check != null)
                     {
                         return check.Name;
                     }
                     return p.
                 });
             }*/
            foreach (var property in includeProperties)
            {
                var propertyInfo = typeof(T).GetProperty(property);
                if (propertyInfo != null)
                {
                    var TP = propertyInfo.PropertyType;
                    Console.WriteLine($"Property Type {TP}");
                    // Construct an expression for the include property
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var propertyExpression = Expression.Property(parameter, property);
                    //var conversion = Expression.Convert(propertyExpression, typeof(T) );
                     Expression<Func<T,object>> lambdaExpression = Expression.Lambda<Func<T,object>>(propertyExpression,parameter);

                    query = query.Include(lambdaExpression);

                    // Call the Include method using the constructed expression
                    var allMethod = typeof(EntityFrameworkQueryableExtensions).GetMethods().Where(m => m.Name == "Include" && m.GetParameters().Length == 2);
                    Console.WriteLine("Printing all methods of EFQueryableExtension");
                    foreach (var method in allMethod)
                    {
                        Console.WriteLine($"Method Name: {method.Name}");
                        var paramsa = method.GetParameters();

                        Console.WriteLine("Printing all the parameter of the method and type of parameter");
                        foreach (var param in paramsa)
                        {
                            Console.WriteLine(param.Name);
                            Console.WriteLine(param.GetType().Name);
                        }


                    }
                    Console.WriteLine("Exiting");
                    var includeMethod = typeof(EntityFrameworkQueryableExtensions).GetMethod("Include", new[] {typeof(Expression<Func<T, object>>) });
                    if(includeMethod != null && query != null)
                    {
                       query = (IQueryable<T>?)includeMethod.Invoke(null, new object[] { query, lambdaExpression });
                    }
                }
            }

            return (IEnumerable<TResult>)await query.ToListAsync();
        }

        private static PropertyInfo? CheckProperty(T p, string property)
        {
            var properti = p.GetType().GetProperty(property);
            if (properti == null)
            {
                return null;
            }
            var name = properti;
            return name;
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