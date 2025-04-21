namespace TaskManagement.Application.DTOs;

public record PerformanceReportResponse(int TotalCompleted,
    double AverageCompletedPerUser, DateTime FromDate, DateTime ToDate);