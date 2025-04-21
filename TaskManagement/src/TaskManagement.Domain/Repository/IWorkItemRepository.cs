using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Repository;

/// <summary>
/// Work item-specific repository interface
/// </summary>
public interface IWorkItemRepository : IRepository<WorkItem>
{
    Task<IEnumerable<WorkItem>> GetByProjectIdAsync(Guid projectId);

    Task<WorkItem?> GetWithDetailsAsync(Guid id);
}