namespace TaskManagement.Domain.Repositories
{
    /// <summary>
    /// Defines a unit of work interface for coordinating transactions across multiple repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for managing Project entities
        /// </summary>
        IProjectRepository Projects { get; }

        /// <summary>
        /// Gets the repository for managing WorkItem entities
        /// </summary>
        IWorkItemRepository WorkItems { get; }

        /// <summary>
        /// Commits all changes made within the unit of work to the underlying data store
        /// </summary>
        /// <returns>The number of affected records</returns>
        /// <exception cref="System.Data.Common.DbException">Thrown when database operation fails</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when concurrency conflicts occur</exception>
        Task<int> CommitAsync();
    }
}