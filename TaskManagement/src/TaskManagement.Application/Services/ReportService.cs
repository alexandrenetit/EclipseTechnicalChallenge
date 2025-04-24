using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Services;

public class ReportService : IReportService
{
    private readonly IWorkItemRepository _workItemRepository;
    private readonly IUserRepository _userRepository;

    public ReportService(IWorkItemRepository workItemRepository, IUserRepository userRepository)
    {
        _workItemRepository = workItemRepository;
        _userRepository = userRepository;
    }

    public async Task<PerformanceReportResponse> GeneratePerformanceReportAsync(Guid userId, DateTime fromDate, DateTime toDate)
    {
        if (userId == Guid.Empty)
            throw new ArgumentNullException("UserId can't be empty");

        // Check if requesting user is a manager
        var requestingUser = await _userRepository.GetByIdAsync(userId);
        if (requestingUser == null || !requestingUser.IsManager)
        {
            throw new UnauthorizedAccessException("Only managers can access performance reports");
        }

        // Get completed work items in date range
        var completedWorkItems = (await _workItemRepository.GetAllAsync())
            .Where(wi => wi.Status == WorkItemStatus.Completed &&
                         wi.DueDate >= fromDate &&
                         wi.DueDate <= toDate)
            .ToList();

        // Get all active users (non-managers who completed tasks)
        var allUsers = await _userRepository.GetAllAsync();
        var activeUsers = allUsers
            .Where(u => !u.IsManager &&
                   completedWorkItems.Any(wi => wi.Id == u.Id))
            .ToList();

        // Calculate average tasks completed per user
        var averageCompleted = activeUsers.Count > 0
            ? (double)completedWorkItems.Count / activeUsers.Count
            : 0;

        return new PerformanceReportResponse(
            completedWorkItems.Count,
            averageCompleted,
            fromDate,
            toDate);
    }
}