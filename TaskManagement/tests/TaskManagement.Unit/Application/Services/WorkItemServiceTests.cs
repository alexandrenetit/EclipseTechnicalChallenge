using Bogus;
using Moq;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Repositories;
using TaskManagement.Domain.Services;
using Xunit;

namespace TaskManagement.Tests.Unit.Application.Services;

/// <summary>
/// Contains unit tests for the WorkItemService class to verify functionality
/// for creating, retrieving, updating, and deleting work items.
/// </summary>
public class WorkItemServiceTests
{
    private readonly Faker _faker;
    private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IWorkItemServiceDomain> _mockDomainWorkItemService;
    private readonly WorkItemService _workItemService;

    /// <summary>
    /// Sets up test dependencies including mocks for repositories, unit of work,
    /// and domain service, and creates the WorkItemService instance for testing.
    /// </summary>
    public WorkItemServiceTests()
    {
        _faker = new Faker();
        _mockWorkItemRepository = new Mock<IWorkItemRepository>();
        _mockProjectRepository = new Mock<IProjectRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockDomainWorkItemService = new Mock<IWorkItemServiceDomain>();

        _workItemService = new WorkItemService(
            _mockWorkItemRepository.Object,
            _mockProjectRepository.Object,
            _mockUnitOfWork.Object,
            _mockDomainWorkItemService.Object);
    }

    /// <summary>
    /// Tests the creation of a work item with a valid request.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Creating a work item with valid request should successfully create and return the work item")]
    public async Task CreateWorkItemAsync_WithValidRequest_ShouldCreateAndReturnWorkItem()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var project = CreateTestProject(ownerId, projectId);

        var request = new CreateWorkItemRequest(
            Title: _faker.Lorem.Sentence(),
            Description: _faker.Lorem.Paragraph(),
            DueDate: _faker.Date.Future(),
            Priority: WorkItemPriority.Medium,
            ProjectId: projectId,
            CreatedBy: ownerId
        );

        WorkItem capturedWorkItem = null;

        _mockProjectRepository
            .Setup(repo => repo.GetByIdAsync(projectId))
            .ReturnsAsync(project);

        _mockDomainWorkItemService
            .Setup(service => service.ValidateWorkItemCreation(project, request.Priority))
            .Verifiable();

        _mockWorkItemRepository
            .Setup(repo => repo.AddAsync(It.IsAny<WorkItem>()))
            .Callback<WorkItem>(workItem => capturedWorkItem = workItem)
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _workItemService.CreateWorkItemAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.DueDate, result.DueDate);
        Assert.Equal(request.Priority, result.Priority);
        Assert.Equal(request.ProjectId, result.ProjectId);
        Assert.Equal(request.CreatedBy, result.CreatedBy);
        Assert.Equal(WorkItemStatus.Pending, result.Status);

        _mockProjectRepository.Verify(repo => repo.GetByIdAsync(projectId), Times.Once);
        _mockDomainWorkItemService.Verify(service => service.ValidateWorkItemCreation(project, request.Priority), Times.Once);
        _mockWorkItemRepository.Verify(repo => repo.AddAsync(It.IsAny<WorkItem>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);

        // Verify work item was created with correct values
        Assert.NotNull(capturedWorkItem);
        Assert.Equal(request.Title, capturedWorkItem.Title);
        Assert.Equal(request.Description, capturedWorkItem.Description);
        Assert.Equal(request.DueDate, capturedWorkItem.DueDate);
        Assert.Equal(request.Priority, capturedWorkItem.Priority);
        Assert.Equal(request.ProjectId, capturedWorkItem.ProjectId);
        Assert.Equal(request.CreatedBy, capturedWorkItem.CreatedBy);
    }

    /// <summary>
    /// Tests the creation of a work item with an invalid request.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Creating a work item with invalid request should throw a validation exception")]
    public async Task CreateWorkItemAsync_WithInvalidRequest_ShouldThrowValidationException()
    {
        // Arrange
        var request = new CreateWorkItemRequest(
            Title: "", // Invalid empty title
            Description: _faker.Lorem.Paragraph(),
            DueDate: _faker.Date.Future(),
            Priority: WorkItemPriority.Medium,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => _workItemService.CreateWorkItemAsync(request)
        );

        // Verify repository methods were never called
        _mockProjectRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockDomainWorkItemService.Verify(service => service.ValidateWorkItemCreation(It.IsAny<Project>(), It.IsAny<WorkItemPriority>()), Times.Never);
        _mockWorkItemRepository.Verify(repo => repo.AddAsync(It.IsAny<WorkItem>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    [Fact(DisplayName = "Creating a work item with non-existing project should throw key not found exception")]
    public async Task CreateWorkItemAsync_WithNonExistingProject_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var request = new CreateWorkItemRequest(
            Title: _faker.Lorem.Sentence(),
            Description: _faker.Lorem.Paragraph(),
            DueDate: _faker.Date.Future(),
            Priority: WorkItemPriority.Medium,
            ProjectId: projectId,
            CreatedBy: Guid.NewGuid()
        );

        _mockProjectRepository
            .Setup(repo => repo.GetByIdAsync(projectId))
            .ReturnsAsync((Project)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _workItemService.CreateWorkItemAsync(request)
        );

        Assert.Contains("Project not found", exception.Message);

        _mockProjectRepository.Verify(repo => repo.GetByIdAsync(projectId), Times.Once);
        _mockDomainWorkItemService.Verify(service => service.ValidateWorkItemCreation(It.IsAny<Project>(), It.IsAny<WorkItemPriority>()), Times.Never);
        _mockWorkItemRepository.Verify(repo => repo.AddAsync(It.IsAny<WorkItem>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    /// <summary>
    /// Tests the creation of a work item that fails domain validation.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Creating a work item that fails domain validation should throw domain exception")]
    public async Task CreateWorkItemAsync_WithDomainValidationFailure_ShouldThrowDomainException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var project = CreateTestProject(ownerId, projectId);

        var request = new CreateWorkItemRequest(
            Title: _faker.Lorem.Sentence(),
            Description: _faker.Lorem.Paragraph(),
            DueDate: _faker.Date.Future(),
            Priority: WorkItemPriority.High,
            ProjectId: projectId,
            CreatedBy: ownerId
        );

        _mockProjectRepository
            .Setup(repo => repo.GetByIdAsync(projectId))
            .ReturnsAsync(project);

        _mockDomainWorkItemService
            .Setup(service => service.ValidateWorkItemCreation(project, request.Priority))
            .Throws(new DomainException("Project cannot have more than 5 high priority work items"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(
            () => _workItemService.CreateWorkItemAsync(request)
        );

        Assert.Contains("Project cannot have more than 5 high priority work items", exception.Message);

        _mockProjectRepository.Verify(repo => repo.GetByIdAsync(projectId), Times.Once);
        _mockDomainWorkItemService.Verify(service => service.ValidateWorkItemCreation(project, request.Priority), Times.Once);
        _mockWorkItemRepository.Verify(repo => repo.AddAsync(It.IsAny<WorkItem>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    /// <summary>
    /// Tests the retrieval of work item details for an existing work item.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Getting details for an existing work item should return the work item with its associated comments and history")]
    public async Task GetWorkItemDetailsAsync_WithExistingWorkItem_ShouldReturnWorkItemDetails()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var workItem = CreateTestWorkItem(Guid.NewGuid(), workItemId);

        // Get the current count of comments and history before adding new ones
        var commentsField = typeof(WorkItem).GetField("_comments",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var comments = (List<WorkItemComment>)commentsField.GetValue(workItem);
        var initialCommentCount = comments.Count;

        var historyField = typeof(WorkItem).GetField("_history",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var history = (List<WorkItemHistory>)historyField.GetValue(workItem);
        var initialHistoryCount = history.Count;

        // Add test items
        var comment1 = CreateTestComment(workItem.Id);
        var comment2 = CreateTestComment(workItem.Id);
        var history1 = CreateTestHistory(workItem.Id);
        var history2 = CreateTestHistory(workItem.Id);

        comments.Add(comment1);
        comments.Add(comment2);
        history.Add(history1);
        history.Add(history2);

        _mockWorkItemRepository
            .Setup(repo => repo.GetWithDetailsAsync(workItemId))
            .ReturnsAsync(workItem);

        // Act
        var result = await _workItemService.GetWorkItemDetailsAsync(workItemId);

        // Assert
        Assert.NotNull(result);
        // ... other assertions ...

        var commentResponses = result.Comments.ToList();
        Assert.Equal(initialCommentCount + 2, commentResponses.Count); // Expect initial + 2 new
        Assert.Contains(commentResponses, c => c.Id == comment1.Id);
        Assert.Contains(commentResponses, c => c.Id == comment2.Id);

        var historyResponses = result.History.ToList();
        Assert.Equal(initialHistoryCount + 2, historyResponses.Count); // Expect initial + 2 new
        Assert.Contains(historyResponses, h => h.Id == history1.Id);
        Assert.Contains(historyResponses, h => h.Id == history2.Id);

        _mockWorkItemRepository.Verify(repo => repo.GetWithDetailsAsync(workItemId), Times.Once);
    }

    /// <summary>
    /// Tests the retrieval of work item details for a non-existing work item.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Getting details for a non-existing work item should throw key not found exception")]
    public async Task GetWorkItemDetailsAsync_WithNonExistingWorkItem_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var workItemId = Guid.NewGuid();

        _mockWorkItemRepository
            .Setup(repo => repo.GetWithDetailsAsync(workItemId))
            .ReturnsAsync((WorkItem)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _workItemService.GetWorkItemDetailsAsync(workItemId)
        );

        Assert.Contains("Work item not found", exception.Message);

        _mockWorkItemRepository.Verify(repo => repo.GetWithDetailsAsync(workItemId), Times.Once);
    }

    /// <summary>
    /// Tests the update of a work item's status with a valid request.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Updating a work item's status with valid request should update and return the work item")]
    public async Task UpdateWorkItemAsync_WithValidStatusUpdate_ShouldUpdateAndReturnWorkItem()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var modifiedBy = Guid.NewGuid();
        var workItem = CreateTestWorkItem(Guid.NewGuid(), workItemId);

        var request = new UpdateWorkItemRequest(
            Title: null,
            Description: null,
            DueDate: null,
            Status: WorkItemStatus.InProgress
        );

        _mockWorkItemRepository
            .Setup(repo => repo.GetByIdAsync(workItemId))
            .ReturnsAsync(workItem);

        _mockUnitOfWork
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _workItemService.UpdateWorkItemAsync(workItemId, request, modifiedBy);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(workItemId, result.Id);
        Assert.Equal(WorkItemStatus.InProgress, result.Status);

        _mockWorkItemRepository.Verify(repo => repo.GetByIdAsync(workItemId), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    /// <summary>
    /// Tests the update of a work item's details with a valid request.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Updating a work item's details with valid request should update and return the work item")]
    public async Task UpdateWorkItemAsync_WithValidDetailsUpdate_ShouldUpdateAndReturnWorkItem()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var modifiedBy = Guid.NewGuid();
        var workItem = CreateTestWorkItem(Guid.NewGuid(), workItemId);
        var originalTitle = workItem.Title;
        var newTitle = "Updated Title";

        var request = new UpdateWorkItemRequest(
            Title: newTitle,
            Description: null,
            DueDate: null,
            Status: null
        );

        _mockWorkItemRepository
            .Setup(repo => repo.GetByIdAsync(workItemId))
            .ReturnsAsync(workItem);

        _mockUnitOfWork
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _workItemService.UpdateWorkItemAsync(workItemId, request, modifiedBy);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(workItemId, result.Id);
        Assert.Equal(newTitle, result.Title);
        Assert.NotEqual(originalTitle, result.Title);

        _mockWorkItemRepository.Verify(repo => repo.GetByIdAsync(workItemId), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    /// <summary>
    /// Tests the update of a work item with an invalid request.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Updating a work item with invalid request should throw validation exception")]
    public async Task UpdateWorkItemAsync_WithInvalidRequest_ShouldThrowValidationException()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var modifiedBy = Guid.NewGuid();

        // Creating an update request with a title that exceeds the max length
        // assuming there's a validation rule for maximum title length
        var request = new UpdateWorkItemRequest(
            Title: new string('x', 1000), // Very long title that would fail validation
            Description: null,
            DueDate: null,
            Status: null
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => _workItemService.UpdateWorkItemAsync(workItemId, request, modifiedBy)
        );

        _mockWorkItemRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    /// <summary>
    /// Tests the update of a work item that does not exist.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Updating a non-existing work item should throw key not found exception")]
    public async Task UpdateWorkItemAsync_WithNonExistingWorkItem_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var modifiedBy = Guid.NewGuid();

        var request = new UpdateWorkItemRequest(
            Title: "Updated Title",
            Description: null,
            DueDate: null,
            Status: null
        );

        _mockWorkItemRepository
            .Setup(repo => repo.GetByIdAsync(workItemId))
            .ReturnsAsync((WorkItem)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _workItemService.UpdateWorkItemAsync(workItemId, request, modifiedBy)
        );

        Assert.Contains("Work item not found", exception.Message);

        _mockWorkItemRepository.Verify(repo => repo.GetByIdAsync(workItemId), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    /// <summary>
    /// Tests the deletion of a completed work item.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Deleting a completed work item should succeed")]
    public async Task DeleteWorkItemAsync_WithCompletedWorkItem_ShouldDeleteWorkItem()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var workItem = CreateTestWorkItem(Guid.NewGuid(), workItemId, WorkItemStatus.Completed);

        _mockWorkItemRepository
            .Setup(repo => repo.GetByIdAsync(workItemId))
            .ReturnsAsync(workItem);

        _mockWorkItemRepository
            .Setup(repo => repo.DeleteAsync(workItem))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(1);

        // Act
        await _workItemService.DeleteWorkItemAsync(workItemId);

        // Assert
        _mockWorkItemRepository.Verify(repo => repo.GetByIdAsync(workItemId), Times.Once);
        _mockWorkItemRepository.Verify(repo => repo.DeleteAsync(workItem), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    /// <summary>
    /// Tests the deletion of a work item that is not completed.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Deleting a non-completed work item should throw invalid operation exception")]
    public async Task DeleteWorkItemAsync_WithNonCompletedWorkItem_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        // Create work item with Pending status
        var workItem = CreateTestWorkItem(Guid.NewGuid(), workItemId, WorkItemStatus.Pending);

        _mockWorkItemRepository
            .Setup(repo => repo.GetByIdAsync(workItemId))
            .ReturnsAsync(workItem);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _workItemService.DeleteWorkItemAsync(workItemId)
        );

        Assert.Contains("Cannot delete pending or in-progress work items", exception.Message);

        _mockWorkItemRepository.Verify(repo => repo.GetByIdAsync(workItemId), Times.Once);
        _mockWorkItemRepository.Verify(repo => repo.DeleteAsync(It.IsAny<WorkItem>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    /// <summary>
    /// Tests the deletion of a work item that does not exist.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Deleting a non-existing work item should throw key not found exception")]
    public async Task DeleteWorkItemAsync_WithNonExistingWorkItem_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var workItemId = Guid.NewGuid();

        _mockWorkItemRepository
            .Setup(repo => repo.GetByIdAsync(workItemId))
            .ReturnsAsync((WorkItem)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _workItemService.DeleteWorkItemAsync(workItemId)
        );

        Assert.Contains("Work item not found", exception.Message);

        _mockWorkItemRepository.Verify(repo => repo.GetByIdAsync(workItemId), Times.Once);
        _mockWorkItemRepository.Verify(repo => repo.DeleteAsync(It.IsAny<WorkItem>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    /// <summary>
    /// Tests the addition of a comment to a work item with a valid request.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Adding a comment with valid request should add the comment and return it")]
    public async Task AddCommentAsync_WithValidRequest_ShouldAddCommentAndReturnResponse()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var workItem = CreateTestWorkItem(Guid.NewGuid(), workItemId);

        var request = new AddCommentRequest(
            Content: _faker.Lorem.Paragraph(),
            AuthorId: authorId
        );

        WorkItemComment capturedComment = null;
        WorkItemHistory capturedHistory = null;

        _mockWorkItemRepository
            .Setup(repo => repo.GetWithDetailsAsync(workItemId))
            .ReturnsAsync(workItem);

        _mockWorkItemRepository
            .Setup(repo => repo.AddCommentAsync(It.IsAny<WorkItemComment>()))
            .Callback<WorkItemComment>(comment => capturedComment = comment)
            .Returns(Task.CompletedTask);

        _mockWorkItemRepository
            .Setup(repo => repo.AddHistoryAsync(It.IsAny<WorkItemHistory>()))
            .Callback<WorkItemHistory>(history => capturedHistory = history)
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _workItemService.AddCommentAsync(workItemId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Content, result.Content);
        Assert.Equal(request.AuthorId, result.AuthorId);

        _mockWorkItemRepository.Verify(repo => repo.GetWithDetailsAsync(workItemId), Times.Once);
        _mockWorkItemRepository.Verify(repo => repo.AddCommentAsync(It.IsAny<WorkItemComment>()), Times.Once);
        _mockWorkItemRepository.Verify(repo => repo.AddHistoryAsync(It.IsAny<WorkItemHistory>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);

        // Verify the captured comment and history
        Assert.NotNull(capturedComment);
        Assert.Equal(request.Content, capturedComment.Content);
        Assert.Equal(request.AuthorId, capturedComment.AuthorId);
        Assert.Equal(workItemId, capturedComment.WorkItemId);

        Assert.NotNull(capturedHistory);
        Assert.Contains("Comment added", capturedHistory.Action);
        Assert.Equal(request.AuthorId, capturedHistory.ModifiedBy);
        Assert.Equal(workItemId, capturedHistory.WorkItemId);
    }

    /// <summary>
    /// Tests the addition of a comment to a work item that does not exist.
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Adding a comment to a non-existing work item should throw key not found exception")]
    public async Task AddCommentAsync_WithNonExistingWorkItem_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var workItemId = Guid.NewGuid();
        var authorId = Guid.NewGuid();

        var request = new AddCommentRequest(
            Content: _faker.Lorem.Paragraph(),
            AuthorId: authorId
        );

        _mockWorkItemRepository
            .Setup(repo => repo.GetWithDetailsAsync(workItemId))
            .ReturnsAsync((WorkItem)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _workItemService.AddCommentAsync(workItemId, request)
        );

        Assert.Contains("Work item not found", exception.Message);

        _mockWorkItemRepository.Verify(repo => repo.GetWithDetailsAsync(workItemId), Times.Once);
        _mockWorkItemRepository.Verify(repo => repo.AddCommentAsync(It.IsAny<WorkItemComment>()), Times.Never);
        _mockWorkItemRepository.Verify(repo => repo.AddHistoryAsync(It.IsAny<WorkItemHistory>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    private Project CreateTestProject(Guid ownerId, Guid? id = null)
    {
        return new Project(
            id ?? Guid.NewGuid(),
            _faker.Company.CompanyName(),
            _faker.Lorem.Paragraph(),
            ownerId
        );
    }

    private WorkItem CreateTestWorkItem(Guid projectId, Guid? id = null, WorkItemStatus status = WorkItemStatus.Pending)
    {
        var workItem = new WorkItem(
            id ?? Guid.NewGuid(),
            _faker.Lorem.Sentence(),
            _faker.Lorem.Paragraph(),
            _faker.Date.Future(),
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

    private WorkItemComment CreateTestComment(Guid workItemId)
    {
        return new WorkItemComment(
            Guid.NewGuid(),
            _faker.Lorem.Sentence(),
            Guid.NewGuid(),
            workItemId
        );
    }

    private WorkItemHistory CreateTestHistory(Guid workItemId)
    {
        return new WorkItemHistory(
            Guid.NewGuid(),
            _faker.Lorem.Sentence(),
            DateTime.UtcNow,
            Guid.NewGuid(),
            workItemId
        );
    }

    private WorkItemPriority GetRandomPriority()
    {
        var priorities = Enum.GetValues(typeof(WorkItemPriority));
        return (WorkItemPriority)priorities.GetValue(_faker.Random.Int(0, priorities.Length - 1));
    }
}