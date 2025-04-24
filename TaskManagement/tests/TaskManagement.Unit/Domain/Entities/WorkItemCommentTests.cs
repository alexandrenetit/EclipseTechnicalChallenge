using FluentAssertions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Entities.Base;
using Xunit;

namespace TaskManagement.Tests.Unit.Domain.Entities;

/// <summary>
/// Tests for the WorkItemComment entity
/// </summary>
public class WorkItemCommentTests
{
    /// <summary>
    /// Tests that constructor creates work item comment with provided values
    /// </summary>
    [Fact(DisplayName = "Constructor should create work item comment with provided values")]
    public void Constructor_WhenCalled_ShouldCreateWorkItemCommentWithProvidedValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var content = "Test comment content";
        var authorId = Guid.NewGuid();
        var workItemId = Guid.NewGuid();

        // Act
        var comment = new WorkItemComment(id, content, authorId, workItemId);

        // Assert
        comment.Id.Should().Be(id);
        comment.Content.Should().Be(content);
        comment.AuthorId.Should().Be(authorId);
        comment.WorkItemId.Should().Be(workItemId);
        comment.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that WorkItemComment inherits from EntityBase
    /// </summary>
    [Fact(DisplayName = "WorkItemComment should inherit from EntityBase")]
    public void WorkItemComment_WhenCreated_ShouldInheritFromEntityBase()
    {
        // Arrange
        var id = Guid.NewGuid();
        var content = "Test comment content";
        var authorId = Guid.NewGuid();
        var workItemId = Guid.NewGuid();

        // Act
        var comment = new WorkItemComment(id, content, authorId, workItemId);

        // Assert
        comment.Should().BeAssignableTo<Entity<Guid>>();
    }

    /// <summary>
    /// Tests that MarkAsUpdated updates the UpdatedAt property
    /// </summary>
    [Fact(DisplayName = "MarkAsUpdated should update UpdatedAt property")]
    public void MarkAsUpdated_WhenCalled_ShouldUpdateUpdatedAtProperty()
    {
        // Arrange
        var comment = new WorkItemComment(
            Guid.NewGuid(),
            "Test comment content",
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        comment.MarkAsUpdated();

        // Assert
        comment.UpdatedAt.Should().NotBeNull();
        comment.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that Equals returns true for comments with same ID
    /// </summary>
    [Fact(DisplayName = "Equals should return true for comments with same ID")]
    public void Equals_WhenComparingCommentsWithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();

        var comment1 = new WorkItemComment(
            id,
            "First comment content",
            Guid.NewGuid(),
            Guid.NewGuid());

        var comment2 = new WorkItemComment(
            id,
            "Second comment content",
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        var result = comment1.Equals(comment2);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that Equals returns false for comments with different IDs
    /// </summary>
    [Fact(DisplayName = "Equals should return false for comments with different IDs")]
    public void Equals_WhenComparingCommentsWithDifferentIds_ShouldReturnFalse()
    {
        // Arrange
        var comment1 = new WorkItemComment(
            Guid.NewGuid(),
            "Test comment content",
            Guid.NewGuid(),
            Guid.NewGuid());

        var comment2 = new WorkItemComment(
            Guid.NewGuid(),
            "Test comment content",
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        var result = comment1.Equals(comment2);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that equality operator returns true for comments with same ID
    /// </summary>
    [Fact(DisplayName = "Equality operator should return true for comments with same ID")]
    public void EqualityOperator_WhenComparingCommentsWithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();

        var comment1 = new WorkItemComment(
            id,
            "First comment content",
            Guid.NewGuid(),
            Guid.NewGuid());

        var comment2 = new WorkItemComment(
            id,
            "Second comment content",
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        var result = comment1 == comment2;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that inequality operator returns true for comments with different IDs
    /// </summary>
    [Fact(DisplayName = "Inequality operator should return true for comments with different IDs")]
    public void InequalityOperator_WhenComparingCommentsWithDifferentIds_ShouldReturnTrue()
    {
        // Arrange
        var comment1 = new WorkItemComment(
            Guid.NewGuid(),
            "Test comment content",
            Guid.NewGuid(),
            Guid.NewGuid());

        var comment2 = new WorkItemComment(
            Guid.NewGuid(),
            "Test comment content",
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        var result = comment1 != comment2;

        // Assert
        result.Should().BeTrue();
    }
}