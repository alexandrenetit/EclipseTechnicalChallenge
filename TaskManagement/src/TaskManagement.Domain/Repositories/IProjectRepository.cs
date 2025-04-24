using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Repositories
{
    /// <summary>
    /// Provides specialized data access operations for Project entities
    /// </summary>
    public interface IProjectRepository : IRepository<Project>
    {
        /// <summary>
        /// Retrieves all projects owned by a specific user
        /// </summary>
        /// <param name="ownerId">The unique identifier of the project owner</param>
        /// <returns>A collection of projects belonging to the specified owner</returns>
        Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId);

        /// <summary>
        /// Retrieves a project by its identifier including all associated work items
        /// </summary>
        /// <param name="id">The unique identifier of the project</param>
        /// <returns>The project with its work items, or null if not found</returns>
        Task<Project?> GetWithWorkItemsAsync(Guid id);
    }
}