namespace TaskManagement.Domain.Repository
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