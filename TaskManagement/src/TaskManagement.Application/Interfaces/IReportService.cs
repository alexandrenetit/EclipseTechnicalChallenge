using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Services;

/// <summary>
/// Interface for ReportService
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Generates a performance report for a given date range.
    /// </summary>
    /// <param name="userId">Id of the user requesting the report.</param>
    /// <param name="fromDate">Start date of the report range.</param>
    /// <param name="toDate">End date of the report range.</param>

    /// <returns>A task that represents the asynchronous operation. The task result contains the performance report response.</returns>
    Task<PerformanceReportResponse> GeneratePerformanceReportAsync(Guid userId, DateTime fromDate, DateTime toDate);
}