using TaskManagement.Domain.Entities.Base;

namespace TaskManagement.Domain.Repository
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    public interface IRepository<T> where T : Entity<Guid>
    {
        Task<T?> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync();

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}