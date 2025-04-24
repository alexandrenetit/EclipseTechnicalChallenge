using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.API.Controllers;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Tests.Integration.Endpoints;

/// <summary>
/// Integration tests for the ReportsController
/// Focus on testing the controller's behavior with mocked dependencies
/// Following Arrange-Act-Assert pattern for clear test structure
/// </summary>
public class ReportsControllerIntegrationTests
{
    // Test data generators using Faker
    private readonly Faker<User> _userFaker;
    private readonly Faker<WorkItem> _workItemFaker;

    public ReportsControllerIntegrationTests()
    {
        // Initialize Faker for User entities
        _userFaker = new Faker<User>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.IsManager, f => f.Random.Bool());

        // Initialize Faker for WorkItem entities
        _workItemFaker = new Faker<WorkItem>()
            .CustomInstantiator(f => new WorkItem(
                Guid.NewGuid(),
                f.Lorem.Sentence(),
                f.Lorem.Paragraph(),
                f.Date.Recent(30),
                f.PickRandom<WorkItemPriority>(),
                Guid.NewGuid(),
                Guid.NewGuid()
            ));
    }

    /// <summary>
    /// Test for GetPerformanceReport method
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "GetPerformanceReport should return OK result with valid manager ID")]
    public async Task GetPerformanceReport_WithValidManagerId_ReturnsOkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromDate = DateTime.UtcNow.AddDays(-30);
        var toDate = DateTime.UtcNow;

        var managerUser = new User(userId, "Manager Name", "manager@example.com", true);

        var regularUserId = Guid.NewGuid();
        var regularUser = new User(regularUserId, "Regular User", "user@example.com", false);

        var workItem = new WorkItem(
            regularUserId,
            "Task 1",
            "Description for task 1",
            DateTime.UtcNow.AddDays(-1),
            WorkItemPriority.Medium,
            Guid.NewGuid(),
            userId
        );
        workItem.UpdateStatus(WorkItemStatus.Completed, userId);

        var completedWorkItems = new List<WorkItem> { workItem };
        var allUsers = new List<User> { managerUser, regularUser };

        var expectedReport = new PerformanceReportResponse(
            1,
            1.0,
            fromDate,
            toDate
        );

        var mockWorkItemRepository = new Mock<IWorkItemRepository>();
        mockWorkItemRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(completedWorkItems);
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(managerUser);
        mockUserRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allUsers);
        var reportService = new ReportService(
            mockWorkItemRepository.Object,
            mockUserRepository.Object);

        var controller = new ReportsController(reportService);

        // Act
        var result = await controller.GetPerformanceReport(userId, fromDate, toDate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<PerformanceReportResponse>(okResult.Value);

        Assert.Equal(expectedReport.TotalCompleted, returnValue.TotalCompleted);
        Assert.Equal(expectedReport.AverageCompletedPerUser, returnValue.AverageCompletedPerUser);
        Assert.Equal(expectedReport.FromDate, returnValue.FromDate);
        Assert.Equal(expectedReport.ToDate, returnValue.ToDate);
    }

    /// <summary>
    /// Test for GetPerformanceReport method when user is not a manager
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "GetPerformanceReport should throw UnauthorizedAccessException for non-manager users")]
    public async Task GetPerformanceReport_WithNonManagerId_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromDate = DateTime.UtcNow.AddDays(-30);
        var toDate = DateTime.UtcNow;

        // Create a non-manager user manually
        var regularUser = new User(userId, "Regular User", "regular@example.com", false);

        // Mock repository responses
        var mockWorkItemRepository = new Mock<IWorkItemRepository>();
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(regularUser);

        // Create the service with mocked repositories
        var reportService = new ReportService(
            mockWorkItemRepository.Object,
            mockUserRepository.Object);

        // Create the controller with mocked service
        var controller = new ReportsController(reportService);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await controller.GetPerformanceReport(userId, fromDate, toDate));
    }

    /// <summary>
    /// Test for GetPerformanceReport method when no date range is specified
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "GetPerformanceReport should use default date range when none specified")]
    public async Task GetPerformanceReport_WithDefaultDateRange_UsesDefaultDates()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Create a manager user manually instead of using Faker
        var managerUser = new User(userId, "Manager Name", "manager@example.com", true);

        // Mock repository responses to return empty collections
        var mockWorkItemRepository = new Mock<IWorkItemRepository>();
        mockWorkItemRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<WorkItem>());

        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(managerUser);
        mockUserRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<User> { managerUser });

        // Create the service with mocked repositories
        var reportService = new ReportService(
            mockWorkItemRepository.Object,
            mockUserRepository.Object);

        // Create the controller with mocked service
        var controller = new ReportsController(reportService);

        // Act
        var result = await controller.GetPerformanceReport(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<PerformanceReportResponse>(okResult.Value);

        // Verify default date range was used (30 days from current date)
        var expectedFromDate = DateTime.UtcNow.AddDays(-30).Date;
        var expectedToDate = DateTime.UtcNow.Date;

        Assert.Equal(expectedFromDate.Date, returnValue.FromDate.Date);
        Assert.Equal(expectedToDate.Date, returnValue.ToDate.Date);
    }

    /// <summary>
    /// Test for GetPerformanceReport method when no tasks are completed
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "GetPerformanceReport should return zero averages when no tasks are completed")]
    public async Task GetPerformanceReport_WithNoCompletedTasks_ReturnsZeroAverages()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fromDate = DateTime.UtcNow.AddDays(-30);
        var toDate = DateTime.UtcNow;

        // Create a manager user manually
        var managerUser = new User(userId, "Manager Name", "manager@example.com", true);

        // Create some non-completed work items manually
        var pendingWorkItems = new List<WorkItem>();
        for (int i = 0; i < 3; i++)
        {
            var workItem = new WorkItem(
                Guid.NewGuid(),
                $"Task {i + 1}",
                $"Description for task {i + 1}",
                DateTime.UtcNow.AddDays(-i), // Different dates within range
                WorkItemPriority.Medium,
                Guid.NewGuid(),
                Guid.NewGuid()
            );
            pendingWorkItems.Add(workItem);
        }

        // Create regular users manually
        var regularUsers = new List<User>
        {
            new User(Guid.NewGuid(), "Regular User 1", "user1@example.com", false),
            new User(Guid.NewGuid(), "Regular User 2", "user2@example.com", false)
        };

        var allUsers = new List<User> { managerUser };
        allUsers.AddRange(regularUsers);

        // Mock repository responses
        var mockWorkItemRepository = new Mock<IWorkItemRepository>();
        mockWorkItemRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(pendingWorkItems);

        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(managerUser);
        mockUserRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allUsers);

        // Create the service with mocked repositories
        var reportService = new ReportService(
            mockWorkItemRepository.Object,
            mockUserRepository.Object);

        // Create the controller with mocked service
        var controller = new ReportsController(reportService);

        // Act
        var result = await controller.GetPerformanceReport(userId, fromDate, toDate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<PerformanceReportResponse>(okResult.Value);
        Assert.Equal(0, returnValue.TotalCompleted);
        Assert.Equal(0, returnValue.AverageCompletedPerUser);
    }

    /// <summary>
    /// Test for GetPerformanceReport method when user ID is empty
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "GetPerformanceReport should throw ArgumentNullException for empty user ID")]
    public async Task GetPerformanceReport_WithEmptyUserId_ThrowsArgumentNullException()
    {
        // Arrange
        var userId = Guid.Empty;
        var fromDate = DateTime.UtcNow.AddDays(-30);
        var toDate = DateTime.UtcNow;

        // Mock repositories
        var mockWorkItemRepository = new Mock<IWorkItemRepository>();
        var mockUserRepository = new Mock<IUserRepository>();

        // Create the service with mocked repositories
        var reportService = new ReportService(
            mockWorkItemRepository.Object,
            mockUserRepository.Object);

        // Create the controller with mocked service
        var controller = new ReportsController(reportService);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await controller.GetPerformanceReport(userId, fromDate, toDate));
    }
}