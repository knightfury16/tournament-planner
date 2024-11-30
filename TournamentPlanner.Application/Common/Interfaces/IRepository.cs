using System.Linq.Expressions;
using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Application.Common.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Adds a new entity to the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task representing the asynchronous operation, with the added entity as the result.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Retrieves all entities from the repository asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a collection of all entities as the result.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Retrieves all entities from the repository that match the specified filter asynchronously.
        /// </summary>
        /// <param name="filter">An expression to filter the entities.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of filtered entities as the result.</returns>
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Retrieves all entities from the repository that match the specified filters asynchronously.
        /// </summary>
        /// <param name="filters">A collection of expressions to filter the entities.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of filtered entities as the result.</returns>
        Task<IEnumerable<T>> GetAllAsync(IEnumerable<Expression<Func<T, bool>>> filters);

        /// <summary>
        /// Retrieves all entities from the repository, including specified related properties, asynchronously.
        ///Further navigation properties to be included can be appended, separated by the '.' character.
        /// Any non-existent properties will be silently ignored by EF Core when building the query.
        /// </summary>
        /// <param name="includeProperties">An array of related properties('.' separable) to include in the query.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of entities including related properties as the result.</returns>
        Task<IEnumerable<T>> GetAllAsync(string[] includeProperties);

        /// <summary>
        /// Retrieves all entities from the repository that match the specified filter and include specified related properties asynchronously.
        ///Further navigation properties to be included can be appended, separated by the '.' character.
        /// Any non-existent properties will be silently ignored by EF Core when building the query.
        /// </summary>
        /// <param name="filter">An expression to filter the entities.</param>
        /// <param name="includeProperties">An array of related properties('.' separable) to include in the query.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of filtered entities including related properties as the result.</returns>
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter, string[] includeProperties);

        /// <summary>
        /// Retrieves all entities from the repository that match the specified filters and include specified related properties asynchronously.
        ///Further navigation properties to be included can be appended, separated by the '.' character.
        /// Any non-existent properties will be silently ignored by EF Core when building the query.
        /// </summary>
        /// <param name="filters">A collection of expressions to filter the entities.</param>
        /// <param name="includeProperties">An array of related properties('.' separable) to include in the query.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of filtered entities including related properties as the result.</returns>
        Task<IEnumerable<T>> GetAllAsync(IEnumerable<Expression<Func<T, bool>>> filters, string[] includeProperties);

        /// <summary>
        /// Retrieves an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>A task representing the asynchronous operation, with the entity as the result, or null if not found.</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <param name="includeProperties">An array of related properties('.' separable) to include in the query.</param>
        /// <returns>A task representing the asynchronous operation, with the entity as the result, or null if not found.</returns>
        Task<T?> GetByIdAsync(int id, string[] includeProperties);

        /// <summary>
        /// Retrieves entities by their name asynchronously.
        /// </summary>
        /// <param name="name">The name of the entities to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of entities matching the name as the result.</returns>
        Task<IEnumerable<T>> GetByNameAsync(string? name);

        /// <summary>
        /// Updates an existing entity in the repository asynchronously.
        /// </summary>
        /// <param name="obj">The entity to update.</param>
        /// <returns>A task representing the asynchronous operation, with the updated entity as the result.</returns>
        Task<T> UpdateAsync(T obj);

        /// <summary>
        /// Updates an existing entity in the repository by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the entity to update.</param>
        /// <param name="obj">The updated entity.</param>
        /// <returns>A task representing the asynchronous operation, with the updated entity as the result.</returns>
        Task<T> UpdateByIdAsync(int id, T obj);

        /// <summary>
        /// Saves all changes made in the repository asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveAsync();

        /// <summary>
        /// Explicitly loads a reference property for the given entity asynchronously.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to load.</typeparam>
        /// <param name="entity">The entity for which to load the property.</param>
        /// <param name="propertyExpression">An expression representing the property to load.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExplicitLoadReferenceAsync<TProperty>(T entity, Expression<Func<T, TProperty?>> propertyExpression) where TProperty : class;

        /// <summary>
        /// Explicitly loads a collection property for the given entity asynchronously.
        /// </summary>
        /// <typeparam name="TProperty">The type of the elements in the collection property.</typeparam>
        /// <param name="entity">The entity for which to load the property.</param>
        /// <param name="propertyExpression">An expression representing the property to load.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExplicitLoadCollectionAsync<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class;

        Task<T?> DeleteByIdAsync(int id);
    }
}