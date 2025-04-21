using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Repositories
{
    /// <summary>
    /// Project-specific repository interface
    /// </summary>
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId);

        Task<Project?> GetWithWorkItemsAsync(Guid id);
    }
}