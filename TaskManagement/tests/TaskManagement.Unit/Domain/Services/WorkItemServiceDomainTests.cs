using FluentAssertions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Services;
using Xunit;

namespace TaskManagement.Tests.Unit.Domain.Services;

/// <summary>
/// Tests for WorkItemServiceDomain to validate work item creation rules
/// </summary>
public class WorkItemServiceDomainTests
{
    private readonly WorkItemServiceDomain _service;

    public WorkItemServiceDomainTests()
    {
        _service = new WorkItemServiceDomain();
    }

    [Fact(DisplayName = "ValidateWorkItemCreation should not throw exception when project has no work items")]
    public void ValidateWorkItemCreation_WithEmptyProject_ShouldNotThrowException()
    {
        // Arrange
        var project = CreateProjectWithWorkItems(0);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.Low))
            .Should().NotThrow();
    }

    [Fact(DisplayName = "ValidateWorkItemCreation should not throw exception when project has less than 20 work items")]
    public void ValidateWorkItemCreation_WithLessThan20WorkItems_ShouldNotThrowException()
    {
        // Arrange
        var project = CreateProjectWithWorkItems(19);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.Low))
            .Should().NotThrow();
    }

    [Fact(DisplayName = "ValidateWorkItemCreation should throw exception when project has 20 work items")]
    public void ValidateWorkItemCreation_With20WorkItems_ShouldThrowDomainException()
    {
        // Arrange
        var project = CreateProjectWithWorkItems(20);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.Low))
            .Should().Throw<DomainException>()
            .WithMessage("Project cannot have more than 20 work items");
    }

    [Fact(DisplayName = "ValidateWorkItemCreation should not throw exception when adding low priority item regardless of existing high priority items")]
    public void ValidateWorkItemCreation_WithLowPriority_ShouldNotThrowException()
    {
        // Arrange
        var project = CreateProjectWithHighPriorityItems(5);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.Low))
            .Should().NotThrow();
    }

    [Fact(DisplayName = "ValidateWorkItemCreation should not throw exception when adding medium priority item regardless of existing high priority items")]
    public void ValidateWorkItemCreation_WithMediumPriority_ShouldNotThrowException()
    {
        // Arrange
        var project = CreateProjectWithHighPriorityItems(5);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.Medium))
            .Should().NotThrow();
    }

    [Fact(DisplayName = "ValidateWorkItemCreation should not throw exception when project has less than 5 high priority items")]
    public void ValidateWorkItemCreation_WithLessThan5HighPriorityItems_ShouldNotThrowException()
    {
        // Arrange
        var project = CreateProjectWithHighPriorityItems(4);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.High))
            .Should().NotThrow();
    }

    [Fact(DisplayName = "ValidateWorkItemCreation should throw exception when project has 5 high priority items")]
    public void ValidateWorkItemCreation_With5HighPriorityItems_ShouldThrowDomainException()
    {
        // Arrange
        var project = CreateProjectWithHighPriorityItems(5);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.High))
            .Should().Throw<DomainException>()
            .WithMessage("Project cannot have more than 5 high priority work items");
    }

    [Fact(DisplayName = "ValidateWorkItemCreation should throw work item count exception when both limits are reached")]
    public void ValidateWorkItemCreation_WithBothLimitsReached_ShouldThrowWorkItemCountException()
    {
        // Arrange
        var project = CreateProjectWithWorkItems(20);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.High))
            .Should().Throw<DomainException>()
            .WithMessage("Project cannot have more than 20 work items");
    }

    #region Helper Methods

    /// <summary>
    /// Creates a project with the specified number of work items
    /// </summary>
    private Project CreateProjectWithWorkItems(int count)
    {
        var project = new Project(
            Guid.NewGuid(),
            "Test Project",
            "Description",
            Guid.NewGuid());

        for (int i = 0; i < count; i++)
        {
            var workItem = CreateWorkItem(project.Id, WorkItemPriority.Low);
            project.AddWorkItem(workItem);
        }

        return project;
    }

    /// <summary>
    /// Creates a project with the specified number of high priority work items
    /// </summary>
    private Project CreateProjectWithHighPriorityItems(int highPriorityCount)
    {
        var project = new Project(
            Guid.NewGuid(),
            "Test Project",
            "Description",
            Guid.NewGuid());

        for (int i = 0; i < highPriorityCount; i++)
        {
            var workItem = CreateWorkItem(project.Id, WorkItemPriority.High);
            project.AddWorkItem(workItem);
        }

        for (int i = 0; i < 5; i++)
        {
            var workItem = CreateWorkItem(project.Id, WorkItemPriority.Low);
            project.AddWorkItem(workItem);
        }

        return project;
    }

    /// <summary>
    /// Creates a work item with the specified priority
    /// </summary>
    private WorkItem CreateWorkItem(Guid projectId, WorkItemPriority priority)
    {
        return new WorkItem(
            Guid.NewGuid(),
            "Test Work Item",
            "Description",
            DateTime.UtcNow.AddDays(7),
            priority,
            projectId,
            Guid.NewGuid());
    }

    #endregion Helper Methods
}