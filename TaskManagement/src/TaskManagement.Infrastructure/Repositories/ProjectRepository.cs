using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Repositories;

/// <summary>
/// Implementation of project repository
/// </summary>
public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Project entity)
    {
        await _context.Projects.AddAsync(entity);
    }

    public async Task DeleteAsync(Project entity)
    {
        _context.Projects.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects.FindAsync(id);
    }

    public async Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId)
    {
        return await _context.Projects
            .Where(p => p.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<Project?> GetWithWorkItemsAsync(Guid id)
    {
        return await _context.Projects
            .Include(p => p.WorkItems)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateAsync(Project entity)
    {
        _context.Projects.Update(entity);
        await Task.CompletedTask;
    }
}