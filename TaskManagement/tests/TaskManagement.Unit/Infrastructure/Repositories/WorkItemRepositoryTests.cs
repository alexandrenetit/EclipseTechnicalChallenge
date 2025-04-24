using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Infrastructure.Repositories;
using Xunit;

namespace TaskManagement.Tests.Unit.Infrastructure.Repositories;

/// <summary>
/// Tests for WorkItemRepository to verify database operations for work items
/// </summary>
public class WorkItemRepositoryTests
{
    [Fact(DisplayName = "AddAsync should successfully add work item to database context")]
    public async Task AddAsync_ShouldAddWorkItemToContext()
    {
        // Arrange
        var dbName = $"AddWorkItem_Test_{Guid.NewGuid()}";
        var dbContext = CreateDbContext(dbName);
        var repository = new WorkItemRepository(dbContext);
        var projectId = Guid.NewGuid();
        var workItem = CreateSampleWorkItem(projectId);

        // Act
        await repository.AddAsync(workItem);
        await dbContext.SaveChangesAsync();

        // Assert
        var freshContext = CreateDbContext(dbName);
        var savedItem = await freshContext.WorkItems.FindAsync(workItem.Id);
        savedItem.Should().NotBeNull();
        savedItem.Title.Should().Be(workItem.Title);
    }

    [Fact(DisplayName = "GetByIdAsync should return work item with included comments")]
    public async Task GetByIdAsync_ShouldReturnWorkItemWithComments()
    {
        // Arrange
        var dbName = $"GetById_Test_{Guid.NewGuid()}";
        var dbContext = CreateDbContext(dbName);

        var projectId = Guid.NewGuid();
        var workItem = CreateSampleWorkItem(projectId);

        // Add a comment using domain method
        workItem.AddComment("Test comment", Guid.NewGuid());

        dbContext.WorkItems.Add(workItem);
        await dbContext.SaveChangesAsync();

        // Create a new repository with a fresh context
        var newContext = CreateDbContext(dbName);
        var repository = new WorkItemRepository(newContext);

        // Act
        var result = await repository.GetByIdAsync(workItem.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Comments.Should().NotBeEmpty();
        result.Comments.Count.Should().Be(1);
    }

    [Fact(DisplayName = "GetByProjectIdAsync should return only work items for specified project")]
    public async Task GetByProjectIdAsync_ShouldReturnOnlyWorkItemsForSpecifiedProject()
    {
        // Arrange
        var dbName = $"GetByProjectId_Test_{Guid.NewGuid()}";
        var dbContext = CreateDbContext(dbName);

        var projectId1 = Guid.NewGuid();
        var projectId2 = Guid.NewGuid();

        // Create work items for two different projects
        var projectItems = new[]
        {
            CreateSampleWorkItem(projectId1),
            CreateSampleWorkItem(projectId1)
        };

        var otherProjectItem = CreateSampleWorkItem(projectId2);

        dbContext.WorkItems.AddRange(projectItems);
        dbContext.WorkItems.Add(otherProjectItem);
        await dbContext.SaveChangesAsync();

        // Create a new repository with a fresh context
        var newContext = CreateDbContext(dbName);
        var repository = new WorkItemRepository(newContext);

        // Act
        var result = await repository.GetByProjectIdAsync(projectId1);

        // Assert
        result.Should().HaveCount(2);
        result.All(w => w.ProjectId == projectId1).Should().BeTrue();
    }

    [Fact(DisplayName = "GetWithDetailsAsync should return work item with comments and history")]
    public async Task GetWithDetailsAsync_ShouldIncludeCommentsAndHistory()
    {
        // Arrange
        var dbName = $"GetWithDetails_Test_{Guid.NewGuid()}";
        var dbContext = CreateDbContext(dbName);

        var projectId = Guid.NewGuid();
        var workItem = CreateSampleWorkItem(projectId);

        // Add a comment using domain method
        workItem.AddComment("Test comment", Guid.NewGuid());

        dbContext.WorkItems.Add(workItem);
        await dbContext.SaveChangesAsync();

        // Create a new repository with a fresh context
        var newContext = CreateDbContext(dbName);
        var repository = new WorkItemRepository(newContext);

        // Act
        var result = await repository.GetWithDetailsAsync(workItem.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Comments.Should().NotBeEmpty();
        result.History.Should().NotBeEmpty(); // Created by constructor
    }

    [Fact(DisplayName = "DeleteAsync should successfully remove work item from database")]
    public async Task DeleteAsync_ShouldRemoveWorkItemFromContext()
    {
        // Arrange
        var dbName = $"Delete_Test_{Guid.NewGuid()}";
        var dbContext = CreateDbContext(dbName);

        var projectId = Guid.NewGuid();
        var workItem = CreateSampleWorkItem(projectId);

        dbContext.WorkItems.Add(workItem);
        await dbContext.SaveChangesAsync();

        // Verify item exists
        var checkContext = CreateDbContext(dbName);
        var existingItem = await checkContext.WorkItems.FindAsync(workItem.Id);
        existingItem.Should().NotBeNull();

        // Create a new repository with a fresh context
        var newContext = CreateDbContext(dbName);
        var repository = new WorkItemRepository(newContext);

        // Get the item to delete from the new context
        var itemToDelete = await newContext.WorkItems.FindAsync(workItem.Id);

        // Act
        await repository.DeleteAsync(itemToDelete);
        await newContext.SaveChangesAsync();

        // Assert - Check with a fresh context
        var verificationContext = CreateDbContext(dbName);
        var deletedItem = await verificationContext.WorkItems.FindAsync(workItem.Id);
        deletedItem.Should().BeNull();
    }

    [Fact(DisplayName = "AddCommentAsync should successfully add comment to database")]
    public async Task AddCommentAsync_ShouldAddCommentToContext()
    {
        // Arrange
        var dbName = $"AddComment_Test_{Guid.NewGuid()}";
        var dbContext = CreateDbContext(dbName);
        var repository = new WorkItemRepository(dbContext);

        var workItemId = Guid.NewGuid();

        var comment = new WorkItemComment(
            Guid.NewGuid(),
            "Test comment content",
            Guid.NewGuid(),
            workItemId
        );

        // Act
        await repository.AddCommentAsync(comment);
        await dbContext.SaveChangesAsync();

        // Assert - Check with a fresh context
        var verificationContext = CreateDbContext(dbName);
        var savedComment = await verificationContext.WorkItemComments.FindAsync(comment.Id);
        savedComment.Should().NotBeNull();
        savedComment.Content.Should().Be("Test comment content");
    }

    [Fact(DisplayName = "AddHistoryAsync should successfully add history record to database")]
    public async Task AddHistoryAsync_ShouldAddHistoryToContext()
    {
        // Arrange
        var dbName = $"AddHistory_Test_{Guid.NewGuid()}";
        var dbContext = CreateDbContext(dbName);
        var repository = new WorkItemRepository(dbContext);

        var workItemId = Guid.NewGuid();

        var history = new WorkItemHistory(
            Guid.NewGuid(),
            "Status changed to Completed",
            DateTime.UtcNow,
            Guid.NewGuid(),
            workItemId
        );

        // Act
        await repository.AddHistoryAsync(history);
        await dbContext.SaveChangesAsync();

        // Assert - Check with a fresh context
        var verificationContext = CreateDbContext(dbName);
        var savedHistory = await verificationContext.WorkItemHistories.FindAsync(history.Id);
        savedHistory.Should().NotBeNull();
        savedHistory.Action.Should().Be("Status changed to Completed");
    }

    [Fact(DisplayName = "UpdateAsync should correctly set entity state to Modified")]
    public async Task UpdateAsync_ShouldCorrectlySetEntityState()
    {
        // Arrange
        var dbName = $"UpdateAsync_Test_{Guid.NewGuid()}";
        var dbContext = CreateDbContext(dbName);
        var repository = new WorkItemRepository(dbContext);
        var workItem = CreateSampleWorkItem(Guid.NewGuid());

        // Add and save to get a clean state
        dbContext.WorkItems.Add(workItem);
        await dbContext.SaveChangesAsync();

        // Detach the entity to simulate getting it from another context
        dbContext.Entry(workItem).State = EntityState.Detached;

        // Act
        await repository.UpdateAsync(workItem);

        // Assert
        var entry = dbContext.Entry(workItem);
        entry.State.Should().Be(EntityState.Modified);
    }

    /// <summary>
    /// Creates an in-memory database context for testing
    /// </summary>
    private ApplicationDbContext CreateDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new ApplicationDbContext(options);
    }

    /// <summary>
    /// Creates a sample work item for testing
    /// </summary>
    private WorkItem CreateSampleWorkItem(Guid projectId)
    {
        return new WorkItem(
            Guid.NewGuid(),
            "Sample Work Item",
            "This is a sample work item",
            DateTime.UtcNow.AddDays(7),
            WorkItemPriority.Medium,
            projectId,
            Guid.NewGuid()
        );
    }
}