using TaskManagement.Domain.Repositories;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IProjectRepository _projectRepository;
        private IWorkItemRepository _workItemRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IProjectRepository Projects => _projectRepository ??= new ProjectRepository(_context);
        public IWorkItemRepository WorkItems => _workItemRepository ??= new WorkItemRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}