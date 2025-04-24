using Bogus;
using FluentAssertions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Entities.Base;
using TaskManagement.Domain.Enums;
using Xunit;

namespace TaskManagement.Tests.Unit.Domain.Entities;

/// <summary>
/// Tests for the WorkItem entity
/// </summary>
public class WorkItemTests
{
    private readonly Faker<WorkItem> _workItemFaker;
    private readonly Faker _faker;

    /// <summary>
    /// Initializes a new instance of the WorkItemTests class
    /// </summary>
    public WorkItemTests()
    {
        _faker = new Faker();

        // Setup Faker for WorkItem entity
        _workItemFaker = new Faker<WorkItem>()
            .CustomInstantiator(f => new WorkItem(
                Guid.NewGuid(),
                f.Lorem.Sentence(),
                f.Lorem.Paragraph(),
                f.Date.Future(),
                (WorkItemPriority)f.Random.Int(0, 2),
                Guid.NewGuid(),
                Guid.NewGuid()));
    }

    /// <summary>
    /// Tests that constructor creates work item with provided values
    /// </summary>
    [Fact(DisplayName = "Constructor should create work item with provided values")]
    public void Constructor_WhenCalled_ShouldCreateWorkItemWithProvidedValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Test Work Item";
        var description = "This is a test work item";
        var dueDate = DateTime.UtcNow.AddDays(7);
        var priority = WorkItemPriority.High;
        var projectId = Guid.NewGuid();
        var createdBy = Guid.NewGuid();

        // Act
        var workItem = new WorkItem(id, title, description, dueDate, priority, projectId, createdBy);

        // Assert
        workItem.Id.Should().Be(id);
        workItem.Title.Should().Be(title);
        workItem.Description.Should().Be(description);
        workItem.DueDate.Should().Be(dueDate);
        workItem.Priority.Should().Be(priority);
        workItem.ProjectId.Should().Be(projectId);
        workItem.CreatedBy.Should().Be(createdBy);
        workItem.Status.Should().Be(WorkItemStatus.Pending);
        workItem.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        workItem.Comments.Should().BeEmpty();
        workItem.History.Should().HaveCount(1);
        workItem.History.First().Action.Should().Be("Work item created");
    }

    /// <summary>
    /// Tests that UpdateStatus updates status and adds history when status changes
    /// </summary>
    [Fact(DisplayName = "UpdateStatus should update status and add history when status changes")]
    public void UpdateStatus_WhenNewStatusDifferent_ShouldUpdateStatusAndAddHistory()
    {
        // Arrange
        var workItem = _workItemFaker.Generate();
        var modifiedBy = Guid.NewGuid();
        var newStatus = WorkItemStatus.InProgress;
        var initialHistoryCount = workItem.History.Count;

        // Act
        workItem.UpdateStatus(newStatus, modifiedBy);

        // Assert
        workItem.Status.Should().Be(newStatus);
        workItem.History.Should().HaveCount(initialHistoryCount + 1);
        workItem.History.Last().Action.Should().Be($"Status changed to {newStatus}");
        workItem.History.Last().ModifiedBy.Should().Be(modifiedBy);
    }

    /// <summary>
    /// Tests that UpdateStatus doesn't update status or add history when status is same
    /// </summary>
    [Fact(DisplayName = "UpdateStatus shouldn't update status or add history when status is same")]
    public void UpdateStatus_WhenNewStatusSame_ShouldNotUpdateStatusOrAddHistory()
    {
        // Arrange
        var workItem = _workItemFaker.Generate();
        var modifiedBy = Guid.NewGuid();
        var initialStatus = workItem.Status;
        var initialHistoryCount = workItem.History.Count;

        // Act
        workItem.UpdateStatus(initialStatus, modifiedBy);

        // Assert
        workItem.Status.Should().Be(initialStatus);
        workItem.History.Should().HaveCount(initialHistoryCount);
    }

    /// <summary>
    /// Tests that UpdateDetails updates all details and adds history when details change
    /// </summary>
    [Fact(DisplayName = "UpdateDetails should update all details and add history when details change")]
    public void UpdateDetails_WhenAllDetailsDifferent_ShouldUpdateAllDetailsAndAddHistory()
    {
        // Arrange
        var workItem = _workItemFaker.Generate();
        var newTitle = "Updated Title";
        var newDescription = "Updated Description";
        var newDueDate = DateTime.UtcNow.AddDays(14);
        var modifiedBy = Guid.NewGuid();
        var initialHistoryCount = workItem.History.Count;
        var initialTitle = workItem.Title;
        var initialDueDate = workItem.DueDate;

        // Act
        workItem.UpdateDetails(newTitle, newDescription, newDueDate, modifiedBy);

        // Assert
        workItem.Title.Should().Be(newTitle);
        workItem.Description.Should().Be(newDescription);
        workItem.DueDate.Should().Be(newDueDate);
        workItem.History.Should().HaveCount(initialHistoryCount + 1);

        var lastHistoryEntry = workItem.History.Last().Action;
        lastHistoryEntry.Should().Contain($"Title changed from '{initialTitle}' to '{newTitle}'");
        lastHistoryEntry.Should().Contain("Description updated");
        lastHistoryEntry.Should().Contain($"Due date changed from {initialDueDate:yyyy-MM-dd} to {newDueDate:yyyy-MM-dd}");
    }

    /// <summary>
    /// Tests that UpdateDetails doesn't update or add history when no details change
    /// </summary>
    [Fact(DisplayName = "UpdateDetails shouldn't update or add history when no details change")]
    public void UpdateDetails_WhenNoDetailsDifferent_ShouldNotUpdateOrAddHistory()
    {
        // Arrange
        var workItem = _workItemFaker.Generate();
        var modifiedBy = Guid.NewGuid();
        var initialHistoryCount = workItem.History.Count;

        // Act
        workItem.UpdateDetails(workItem.Title, workItem.Description, workItem.DueDate, modifiedBy);

        // Assert
        workItem.History.Should().HaveCount(initialHistoryCount);
    }

    /// <summary>
    /// Tests that UpdateDetails updates only title and adds history when only title changes
    /// </summary>
    [Fact(DisplayName = "UpdateDetails should update only title and add history when only title changes")]
    public void UpdateDetails_WhenOnlyTitleDifferent_ShouldUpdateTitleAndAddHistoryWithTitleChangeOnly()
    {
        // Arrange
        var workItem = _workItemFaker.Generate();
        var newTitle = "Updated Title";
        var modifiedBy = Guid.NewGuid();
        var initialHistoryCount = workItem.History.Count;
        var initialTitle = workItem.Title;

        // Act
        workItem.UpdateDetails(newTitle, workItem.Description, workItem.DueDate, modifiedBy);

        // Assert
        workItem.Title.Should().Be(newTitle);
        workItem.History.Should().HaveCount(initialHistoryCount + 1);
        workItem.History.Last().Action.Should().Be($"Title changed from '{initialTitle}' to '{newTitle}'");
    }

    /// <summary>
    /// Tests that AddComment adds comment and history
    /// </summary>
    [Fact(DisplayName = "AddComment should add comment and history")]
    public void AddComment_WhenCalled_ShouldAddCommentAndHistory()
    {
        // Arrange
        var workItem = _workItemFaker.Generate();
        var content = "This is a test comment";
        var authorId = Guid.NewGuid();
        var initialCommentsCount = workItem.Comments.Count;
        var initialHistoryCount = workItem.History.Count;

        // Act
        workItem.AddComment(content, authorId);

        // Assert
        workItem.Comments.Should().HaveCount(initialCommentsCount + 1);
        workItem.Comments.Last().Content.Should().Be(content);
        workItem.Comments.Last().AuthorId.Should().Be(authorId);
        workItem.Comments.Last().WorkItemId.Should().Be(workItem.Id);

        workItem.History.Should().HaveCount(initialHistoryCount + 1);
        workItem.History.Last().Action.Should().Be($"Comment added: {content}");
        workItem.History.Last().ModifiedBy.Should().Be(authorId);
    }

    /// <summary>
    /// Tests that RecordHistory uses CreatedBy as modifier when none provided
    /// </summary>
    [Fact(DisplayName = "RecordHistory should use CreatedBy as modifier when none provided")]
    public void RecordHistory_WhenCalledWithoutModifiedBy_ShouldUseCreatedByAsModifier()
    {
        // Arrange
        var workItem = _workItemFaker.Generate();
        var initialHistoryCount = workItem.History.Count;
        var action = "Test action";

        // Use reflection to call private method
        var recordHistoryMethod = typeof(WorkItem).GetMethod("RecordHistory",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        recordHistoryMethod.Invoke(workItem, new object[] { action, null });

        // Assert
        workItem.History.Should().HaveCount(initialHistoryCount + 1);
        workItem.History.Last().Action.Should().Be(action);
        workItem.History.Last().ModifiedBy.Should().Be(workItem.CreatedBy);
    }

    /// <summary>
    /// Tests that RecordHistory uses provided modifier when specified
    /// </summary>
    [Fact(DisplayName = "RecordHistory should use provided modifier when specified")]
    public void RecordHistory_WhenCalledWithModifiedBy_ShouldUseProvidedModifier()
    {
        // Arrange
        var workItem = _workItemFaker.Generate();
        var initialHistoryCount = workItem.History.Count;
        var action = "Test action";
        var modifiedBy = Guid.NewGuid();

        // Use reflection to call private method
        var recordHistoryMethod = typeof(WorkItem).GetMethod("RecordHistory",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        recordHistoryMethod.Invoke(workItem, new object[] { action, modifiedBy });

        // Assert
        workItem.History.Should().HaveCount(initialHistoryCount + 1);
        workItem.History.Last().Action.Should().Be(action);
        workItem.History.Last().ModifiedBy.Should().Be(modifiedBy);
    }

    /// <summary>
    /// Tests that WorkItem inherits from EntityBase
    /// </summary>
    [Fact(DisplayName = "WorkItem should inherit from EntityBase")]
    public void WorkItem_WhenCreated_ShouldInheritFromEntityBase()
    {
        // Arrange
        var workItem = _workItemFaker.Generate();

        // Act & Assert
        workItem.Should().BeAssignableTo<Entity<Guid>>();
    }

    /// <summary>
    /// Tests that Equals returns true for work items with same ID
    /// </summary>
    [Fact(DisplayName = "Equals should return true for work items with same ID")]
    public void Equals_WhenComparingWorkItemsWithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var createdBy = Guid.NewGuid();

        var workItem1 = new WorkItem(
            id,
            "Work Item 1",
            "Description 1",
            DateTime.UtcNow.AddDays(1),
            WorkItemPriority.Low,
            projectId,
            createdBy);

        var workItem2 = new WorkItem(
            id,
            "Work Item 2",
            "Description 2",
            DateTime.UtcNow.AddDays(2),
            WorkItemPriority.High,
            projectId,
            createdBy);

        // Act
        var result = workItem1.Equals(workItem2);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that Equals returns false for work items with different IDs
    /// </summary>
    [Fact(DisplayName = "Equals should return false for work items with different IDs")]
    public void Equals_WhenComparingWorkItemsWithDifferentIds_ShouldReturnFalse()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var createdBy = Guid.NewGuid();

        var workItem1 = new WorkItem(
            Guid.NewGuid(),
            "Work Item 1",
            "Description 1",
            DateTime.UtcNow.AddDays(1),
            WorkItemPriority.Low,
            projectId,
            createdBy);

        var workItem2 = new WorkItem(
            Guid.NewGuid(),
            "Work Item 1",
            "Description 1",
            DateTime.UtcNow.AddDays(1),
            WorkItemPriority.Low,
            projectId,
            createdBy);

        // Act
        var result = workItem1.Equals(workItem2);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that equality operator returns true for work items with same ID
    /// </summary>
    [Fact(DisplayName = "Equality operator should return true for work items with same ID")]
    public void EqualityOperator_WhenComparingWorkItemsWithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var createdBy = Guid.NewGuid();

        var workItem1 = new WorkItem(
            id,
            "Work Item 1",
            "Description 1",
            DateTime.UtcNow.AddDays(1),
            WorkItemPriority.Low,
            projectId,
            createdBy);

        var workItem2 = new WorkItem(
            id,
            "Work Item 2",
            "Description 2",
            DateTime.UtcNow.AddDays(2),
            WorkItemPriority.High,
            projectId,
            createdBy);

        // Act
        var result = workItem1 == workItem2;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that inequality operator returns true for work items with different IDs
    /// </summary>
    [Fact(DisplayName = "Inequality operator should return true for work items with different IDs")]
    public void InequalityOperator_WhenComparingWorkItemsWithDifferentIds_ShouldReturnTrue()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var createdBy = Guid.NewGuid();

        var workItem1 = new WorkItem(
            Guid.NewGuid(),
            "Work Item 1",
            "Description 1",
            DateTime.UtcNow.AddDays(1),
            WorkItemPriority.Low,
            projectId,
            createdBy);

        var workItem2 = new WorkItem(
            Guid.NewGuid(),
            "Work Item 1",
            "Description 1",
            DateTime.UtcNow.AddDays(1),
            WorkItemPriority.Low,
            projectId,
            createdBy);

        // Act
        var result = workItem1 != workItem2;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that MarkAsUpdated updates the UpdatedAt property
    /// </summary>
    [Fact(DisplayName = "MarkAsUpdated should update the UpdatedAt property")]
    public void MarkAsUpdated_WhenCalled_ShouldUpdateUpdatedAtProperty()
    {
        // Arrange
        var workItem = _workItemFaker.Generate();

        // Act
        workItem.MarkAsUpdated();

        // Assert
        workItem.UpdatedAt.Should().NotBeNull();
        workItem.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}