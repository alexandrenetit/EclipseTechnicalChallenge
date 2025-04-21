using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repository;

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

    public async Task<PerformanceReportResponse> GeneratePerformanceReportAsync(DateTime fromDate, DateTime toDate)
    {
        var completedWorkItems = (await _workItemRepository.GetAllAsync())
            .Where(wi => wi.Status == WorkItemStatus.Completed &&
                         wi.DueDate >= fromDate &&
                         wi.DueDate <= toDate)
            .ToList();

        var users = await _userRepository.GetAllAsync();
        var activeUsers = users.Count(u => u.IsManager);

        var averageCompleted = activeUsers > 0
            ? (double)completedWorkItems.Count / activeUsers
            : 0;

        return new PerformanceReportResponse(
            completedWorkItems.Count,
            averageCompleted,
            fromDate,
            toDate);
    }
}