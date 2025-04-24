using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Repositories;

/// <summary>
/// Provides specialized data access operations for WorkItem entities and their related aggregates
/// </summary>
public interface IWorkItemRepository : IRepository<WorkItem>
{
    /// <summary>
    /// Retrieves all work items associated with a specific project
    /// </summary>
    /// <param name="projectId">The unique identifier of the project</param>
    /// <returns>A collection of work items belonging to the specified project</returns>
    Task<IEnumerable<WorkItem>> GetByProjectIdAsync(Guid projectId);

    /// <summary>
    /// Retrieves a work item by its identifier including all related details (comments, history, etc.)
    /// </summary>
    /// <param name="id">The unique identifier of the work item</param>
    /// <returns>The fully populated work item, or null if not found</returns>
    Task<WorkItem?> GetWithDetailsAsync(Guid id);

    /// <summary>
    /// Adds a new comment to a work item
    /// </summary>
    /// <param name="comment">The comment entity to add</param>
    /// <returns>A task representing the asynchronous operation</returns>
    /// <exception cref="ArgumentException">Thrown when comment is invalid</exception>
    Task AddCommentAsync(WorkItemComment comment);

    /// <summary>
    /// Records a history entry for a work item
    /// </summary>
    /// <param name="history">The history entity to add</param>
    /// <returns>A task representing the asynchronous operation</returns>
    /// <exception cref="ArgumentException">Thrown when history data is invalid</exception>
    Task AddHistoryAsync(WorkItemHistory history);
}