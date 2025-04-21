namespace TaskManagement.Domain.Repositories
{
    /// <summary>
    /// Unit of work interface for transaction management
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepository Projects { get; }
        IWorkItemRepository WorkItems { get; }

        Task<int> CommitAsync();
    }
}