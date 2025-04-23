using FluentAssertions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Services;
using Xunit;

namespace TaskManagement.Tests.Unit.Domain.Services;

public class WorkItemServiceDomainTests
{
    private readonly WorkItemServiceDomain _service;

    public WorkItemServiceDomainTests()
    {
        _service = new WorkItemServiceDomain();
    }

    [Fact]
    public void ValidateWorkItemCreation_WithEmptyProject_ShouldNotThrowException()
    {
        // Arrange
        var project = CreateProjectWithWorkItems(0);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.Low))
            .Should().NotThrow();
    }

    [Fact]
    public void ValidateWorkItemCreation_WithLessThan20WorkItems_ShouldNotThrowException()
    {
        // Arrange
        var project = CreateProjectWithWorkItems(19);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.Low))
            .Should().NotThrow();
    }

    [Fact]
    public void ValidateWorkItemCreation_With20WorkItems_ShouldThrowDomainException()
    {
        // Arrange
        var project = CreateProjectWithWorkItems(20);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.Low))
            .Should().Throw<DomainException>()
            .WithMessage("Project cannot have more than 20 work items");
    }

    [Fact]
    public void ValidateWorkItemCreation_WithLowPriority_ShouldNotThrowException()
    {
        // Arrange
        var project = CreateProjectWithHighPriorityItems(5);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.Low))
            .Should().NotThrow();
    }

    [Fact]
    public void ValidateWorkItemCreation_WithMediumPriority_ShouldNotThrowException()
    {
        // Arrange
        var project = CreateProjectWithHighPriorityItems(5);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.Medium))
            .Should().NotThrow();
    }

    [Fact]
    public void ValidateWorkItemCreation_WithLessThan5HighPriorityItems_ShouldNotThrowException()
    {
        // Arrange
        var project = CreateProjectWithHighPriorityItems(4);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.High))
            .Should().NotThrow();
    }

    [Fact]
    public void ValidateWorkItemCreation_With5HighPriorityItems_ShouldThrowDomainException()
    {
        // Arrange
        var project = CreateProjectWithHighPriorityItems(5);

        // Act & Assert
        _service.Invoking(s => s.ValidateWorkItemCreation(project, WorkItemPriority.High))
            .Should().Throw<DomainException>()
            .WithMessage("Project cannot have more than 5 high priority work items");
    }

    [Fact]
    public void ValidateWorkItemCreation_WithBothLimitsReached_ShouldThrowWorkItemCountException()
    {
        // Arrange
        // Create a project with 20 work items (max limit)
        var project = CreateProjectWithWorkItems(20);

        // Act & Assert
        // The work item count exception should take precedence
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

        // Add the specified number of work items to the project
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

        // Add high priority work items
        for (int i = 0; i < highPriorityCount; i++)
        {
            var workItem = CreateWorkItem(project.Id, WorkItemPriority.High);
            project.AddWorkItem(workItem);
        }

        // Add some low priority items to make the test more realistic
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