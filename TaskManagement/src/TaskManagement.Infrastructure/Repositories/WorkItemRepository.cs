using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Repositories;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly ApplicationDbContext _context;

    public WorkItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(WorkItem entity)
    {
        await _context.WorkItems.AddAsync(entity);
    }

    public async Task DeleteAsync(WorkItem entity)
    {
        _context.WorkItems.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<WorkItem>> GetAllAsync()
    {
        return await _context.WorkItems.ToListAsync();
    }

    public async Task<WorkItem?> GetByIdAsync(Guid id)
    {
        return await _context.WorkItems
            .Include(wi => wi.Comments)
            .FirstOrDefaultAsync(wi => wi.Id == id);
    }

    public async Task<IEnumerable<WorkItem>> GetByProjectIdAsync(Guid projectId)
    {
        return await _context.WorkItems
            .Where(wi => wi.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<WorkItem?> GetWithDetailsAsync(Guid id)
    {
        return await _context.WorkItems
            .Include(wi => wi.Comments)
            .Include(wi => wi.History)
            .AsTracking()
            .FirstOrDefaultAsync(wi => wi.Id == id);
    }

    public async Task UpdateAsync(WorkItem entity)
    {
        _context.Entry(entity).State = EntityState.Modified;

        _context.Entry(entity).Collection(e => e.Comments).IsModified = true;
        _context.Entry(entity).Collection(e => e.History).IsModified = true;

        await Task.CompletedTask;
    }

    public async Task AddCommentAsync(WorkItemComment comment)
    {
        await _context.WorkItemComments.AddAsync(comment);
    }

    public async Task AddHistoryAsync(WorkItemHistory history)
    {
        await _context.WorkItemHistories.AddAsync(history);
    }
}