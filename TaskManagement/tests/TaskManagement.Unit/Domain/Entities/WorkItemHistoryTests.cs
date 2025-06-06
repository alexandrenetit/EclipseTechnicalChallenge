﻿using Bogus;
using FluentAssertions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Entities.Base;
using Xunit;

namespace TaskManagement.Tests.Unit.Domain.Entities;

/// <summary>
/// Tests for the WorkItemHistory entity
/// </summary>
public class WorkItemHistoryTests
{
    private readonly Faker _faker;

    /// <summary>
    /// Initializes a new instance of the WorkItemHistoryTests class
    /// </summary>
    public WorkItemHistoryTests()
    {
        _faker = new Faker();
    }

    /// <summary>
    /// Tests that constructor creates work item history with provided values
    /// </summary>
    [Fact(DisplayName = "Constructor should create work item history with provided values")]
    public void Constructor_WhenCalled_ShouldCreateWorkItemHistoryWithProvidedValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var action = "Status changed to Completed";
        var timestamp = DateTime.UtcNow;
        var modifiedBy = Guid.NewGuid();
        var workItemId = Guid.NewGuid();

        // Act
        var history = new WorkItemHistory(id, action, timestamp, modifiedBy, workItemId);

        // Assert
        history.Id.Should().Be(id);
        history.Action.Should().Be(action);
        history.Timestamp.Should().Be(timestamp);
        history.ModifiedBy.Should().Be(modifiedBy);
        history.WorkItemId.Should().Be(workItemId);
        history.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that WorkItemHistory inherits from EntityBase
    /// </summary>
    [Fact(DisplayName = "WorkItemHistory should inherit from EntityBase")]
    public void WorkItemHistory_WhenCreated_ShouldInheritFromEntityBase()
    {
        // Arrange
        var id = Guid.NewGuid();
        var action = "Status changed to Completed";
        var timestamp = DateTime.UtcNow;
        var modifiedBy = Guid.NewGuid();
        var workItemId = Guid.NewGuid();

        // Act
        var history = new WorkItemHistory(id, action, timestamp, modifiedBy, workItemId);

        // Assert
        history.Should().BeAssignableTo<Entity<Guid>>();
    }

    /// <summary>
    /// Tests that MarkAsUpdated updates the UpdatedAt property
    /// </summary>
    [Fact(DisplayName = "MarkAsUpdated should update UpdatedAt property")]
    public void MarkAsUpdated_WhenCalled_ShouldUpdateUpdatedAtProperty()
    {
        // Arrange
        var history = new WorkItemHistory(
            Guid.NewGuid(),
            "Status changed to Completed",
            DateTime.UtcNow,
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        history.MarkAsUpdated();

        // Assert
        history.UpdatedAt.Should().NotBeNull();
        history.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that Equals returns true for histories with same ID
    /// </summary>
    [Fact(DisplayName = "Equals should return true for histories with same ID")]
    public void Equals_WhenComparingHistoriesWithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();

        var history1 = new WorkItemHistory(
            id,
            "Status changed to In Progress",
            DateTime.UtcNow.AddHours(-1),
            Guid.NewGuid(),
            Guid.NewGuid());

        var history2 = new WorkItemHistory(
            id,
            "Status changed to Completed",
            DateTime.UtcNow,
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        var result = history1.Equals(history2);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that Equals returns false for histories with different IDs
    /// </summary>
    [Fact(DisplayName = "Equals should return false for histories with different IDs")]
    public void Equals_WhenComparingHistoriesWithDifferentIds_ShouldReturnFalse()
    {
        // Arrange
        var history1 = new WorkItemHistory(
            Guid.NewGuid(),
            "Status changed to In Progress",
            DateTime.UtcNow,
            Guid.NewGuid(),
            Guid.NewGuid());

        var history2 = new WorkItemHistory(
            Guid.NewGuid(),
            "Status changed to In Progress",
            DateTime.UtcNow,
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        var result = history1.Equals(history2);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that equality operator returns true for histories with same ID
    /// </summary>
    [Fact(DisplayName = "Equality operator should return true for histories with same ID")]
    public void EqualityOperator_WhenComparingHistoriesWithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();

        var history1 = new WorkItemHistory(
            id,
            "Status changed to In Progress",
            DateTime.UtcNow.AddHours(-1),
            Guid.NewGuid(),
            Guid.NewGuid());

        var history2 = new WorkItemHistory(
            id,
            "Status changed to Completed",
            DateTime.UtcNow,
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        var result = history1 == history2;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that inequality operator returns true for histories with different IDs
    /// </summary>
    [Fact(DisplayName = "Inequality operator should return true for histories with different IDs")]
    public void InequalityOperator_WhenComparingHistoriesWithDifferentIds_ShouldReturnTrue()
    {
        // Arrange
        var history1 = new WorkItemHistory(
            Guid.NewGuid(),
            "Status changed to In Progress",
            DateTime.UtcNow,
            Guid.NewGuid(),
            Guid.NewGuid());

        var history2 = new WorkItemHistory(
            Guid.NewGuid(),
            "Status changed to In Progress",
            DateTime.UtcNow,
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        var result = history1 != history2;

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that GetHashCode returns same value for histories with same ID
    /// </summary>
    [Fact(DisplayName = "GetHashCode should return same value for histories with same ID")]
    public void GetHashCode_WhenCalledOnHistoriesWithSameId_ShouldReturnSameValue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        var history1 = new WorkItemHistory(
            id,
            "Status changed to In Progress",
            DateTime.UtcNow.AddHours(-1),
            Guid.NewGuid(),
            Guid.NewGuid());

        // Use reflection to set the same CreatedAt value for deterministic hash code
        typeof(Entity<Guid>).GetProperty("CreatedAt")!
            .SetValue(history1, createdAt);

        var history2 = new WorkItemHistory(
            id,
            "Status changed to Completed",
            DateTime.UtcNow,
            Guid.NewGuid(),
            Guid.NewGuid());

        typeof(Entity<Guid>).GetProperty("CreatedAt")!
            .SetValue(history2, createdAt);

        // Act
        var hashCode1 = history1.GetHashCode();
        var hashCode2 = history2.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }
}