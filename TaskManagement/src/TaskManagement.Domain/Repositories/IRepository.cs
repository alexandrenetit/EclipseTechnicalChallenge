using TaskManagement.Domain.Entities.Base;

namespace TaskManagement.Domain.Repositories
{
    /// <summary>
    /// Defines the basic CRUD operations for entities in the system
    /// </summary>
    /// <typeparam name="T">The entity type, must inherit from Entity<Guid></typeparam>
    public interface IRepository<T> where T : Entity<Guid>
    {
        /// <summary>
        /// Retrieves an entity by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the entity</param>
        /// <returns>The entity if found, otherwise null</returns>
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all entities of type T from the repository
        /// </summary>
        /// <returns>A collection of all entities</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Adds a new entity to the repository
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity in the repository
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Removes an entity from the repository
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task DeleteAsync(T entity);
    }
}