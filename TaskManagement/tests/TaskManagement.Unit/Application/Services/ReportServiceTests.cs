using Bogus;
using Moq;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repositories;
using Xunit;

namespace TaskManagement.Tests.Unit.Application.Services;

public class ReportServiceTests
{
    private readonly Faker _faker;
    private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly ReportService _reportService;

    public ReportServiceTests()
    {
        _faker = new Faker();
        _mockWorkItemRepository = new Mock<IWorkItemRepository>();
        _mockUserRepository = new Mock<IUserRepository>();

        _reportService = new ReportService(
            _mockWorkItemRepository.Object,
            _mockUserRepository.Object);
    }

    [Fact]
    public async Task GeneratePerformanceReportAsync_WithCompletedWorkItems_ShouldReturnCorrectReport()
    {
        // Arrange
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 12, 31);

        // Create a list of work items with mixed statuses and dates
        var allWorkItems = new List<WorkItem>
        {
            // Completed work items within date range (should be counted)
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.Completed, new DateTime(2023, 3, 15)),
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.Completed, new DateTime(2023, 6, 20)),
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.Completed, new DateTime(2023, 9, 10)),

            // Completed work items outside date range (should NOT be counted)
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.Completed, new DateTime(2022, 12, 20)),
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.Completed, new DateTime(2024, 1, 5)),

            // Non-completed work items within date range (should NOT be counted)
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.Pending, new DateTime(2023, 4, 15)),
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.InProgress, new DateTime(2023, 8, 22))
        };

        // Create users with mixed roles
        var allUsers = new List<User>
        {
            CreateTestUser(true),  // Manager
            CreateTestUser(true),  // Manager
            CreateTestUser(false), // Non-manager
            CreateTestUser(false)  // Non-manager
        };

        _mockWorkItemRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allWorkItems);

        _mockUserRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allUsers);

        // Expected values
        int expectedCompletedCount = 3; // Only completed items within date range
        int activeManagersCount = 2;    // Only managers
        double expectedAverage = (double)expectedCompletedCount / activeManagersCount;

        // Act
        var result = await _reportService.GeneratePerformanceReportAsync(fromDate, toDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCompletedCount, result.TotalCompleted);
        Assert.Equal(expectedAverage, result.AverageCompletedPerUser);
        Assert.Equal(fromDate, result.FromDate);
        Assert.Equal(toDate, result.ToDate);

        _mockWorkItemRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GeneratePerformanceReportAsync_WithNoCompletedWorkItems_ShouldReturnZeroCounts()
    {
        // Arrange
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 12, 31);

        // Create work items but none that match our criteria
        var allWorkItems = new List<WorkItem>
        {
            // All items are either not completed or outside date range
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.Pending, new DateTime(2023, 5, 10)),
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.InProgress, new DateTime(2023, 7, 22)),
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.Completed, new DateTime(2022, 10, 15))
        };

        // Create some users
        var allUsers = new List<User>
        {
            CreateTestUser(true),
            CreateTestUser(false)
        };

        _mockWorkItemRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allWorkItems);

        _mockUserRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allUsers);

        // Expected values
        int expectedCompletedCount = 0; // No completed items within date range
        double expectedAverage = 0;     // No completed items means average is 0

        // Act
        var result = await _reportService.GeneratePerformanceReportAsync(fromDate, toDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCompletedCount, result.TotalCompleted);
        Assert.Equal(expectedAverage, result.AverageCompletedPerUser);
        Assert.Equal(fromDate, result.FromDate);
        Assert.Equal(toDate, result.ToDate);

        _mockWorkItemRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GeneratePerformanceReportAsync_WithNoActiveManagers_ShouldReturnZeroAverage()
    {
        // Arrange
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 12, 31);

        // Create work items with some completed in date range
        var allWorkItems = new List<WorkItem>
        {
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.Completed, new DateTime(2023, 5, 10)),
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.Completed, new DateTime(2023, 7, 22))
        };

        // Create users but none are managers
        var allUsers = new List<User>
        {
            CreateTestUser(false),
            CreateTestUser(false)
        };

        _mockWorkItemRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allWorkItems);

        _mockUserRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allUsers);

        // Expected values
        int expectedCompletedCount = 2; // Two completed items within date range
        double expectedAverage = 0;     // No managers means average should be 0

        // Act
        var result = await _reportService.GeneratePerformanceReportAsync(fromDate, toDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCompletedCount, result.TotalCompleted);
        Assert.Equal(expectedAverage, result.AverageCompletedPerUser);
        Assert.Equal(fromDate, result.FromDate);
        Assert.Equal(toDate, result.ToDate);

        _mockWorkItemRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GeneratePerformanceReportAsync_WithEmptyRepositories_ShouldReturnZeroCounts()
    {
        // Arrange
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 12, 31);

        // Empty repositories
        var allWorkItems = new List<WorkItem>();
        var allUsers = new List<User>();

        _mockWorkItemRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allWorkItems);

        _mockUserRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allUsers);

        // Expected values
        int expectedCompletedCount = 0;
        double expectedAverage = 0;

        // Act
        var result = await _reportService.GeneratePerformanceReportAsync(fromDate, toDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCompletedCount, result.TotalCompleted);
        Assert.Equal(expectedAverage, result.AverageCompletedPerUser);
        Assert.Equal(fromDate, result.FromDate);
        Assert.Equal(toDate, result.ToDate);

        _mockWorkItemRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GeneratePerformanceReportAsync_WithEqualStartAndEndDates_ShouldReturnCorrectReport()
    {
        // Arrange
        var sameDate = new DateTime(2023, 6, 15);

        // Create work items - one completed on the exact date
        var allWorkItems = new List<WorkItem>
        {
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.Completed, sameDate),
            CreateTestWorkItem(Guid.NewGuid(), WorkItemStatus.Completed, new DateTime(2023, 6, 14)) // day before
        };

        // Create users
        var allUsers = new List<User>
        {
            CreateTestUser(true) // One manager
        };

        _mockWorkItemRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allWorkItems);

        _mockUserRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allUsers);

        // Expected values
        int expectedCompletedCount = 1; // Only one on the exact date
        double expectedAverage = 1.0;   // 1 item / 1 manager

        // Act
        var result = await _reportService.GeneratePerformanceReportAsync(sameDate, sameDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCompletedCount, result.TotalCompleted);
        Assert.Equal(expectedAverage, result.AverageCompletedPerUser);
        Assert.Equal(sameDate, result.FromDate);
        Assert.Equal(sameDate, result.ToDate);

        _mockWorkItemRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    private WorkItem CreateTestWorkItem(Guid projectId, WorkItemStatus status = WorkItemStatus.Pending, DateTime? dueDate = null)
    {
        var workItem = new WorkItem(
            Guid.NewGuid(),
            _faker.Lorem.Sentence(),
            _faker.Lorem.Paragraph(),
            dueDate ?? _faker.Date.Future(),
            GetRandomPriority(),
            projectId,
            Guid.NewGuid()
        );

        // If we need to set a specific status, use reflection to change it
        if (status != WorkItemStatus.Pending)
        {
            var statusProperty = typeof(WorkItem).GetProperty("Status");
            statusProperty.SetValue(workItem, status);
        }

        return workItem;
    }

    private User CreateTestUser(bool isManager)
    {
        return new User(
            Guid.NewGuid(),
            _faker.Name.FullName(),
            _faker.Internet.Email(),
            isManager
        );
    }

    private WorkItemPriority GetRandomPriority()
    {
        var priorities = Enum.GetValues(typeof(WorkItemPriority));
        return (WorkItemPriority)priorities.GetValue(_faker.Random.Int(0, priorities.Length - 1));
    }
}