using Bogus;
using FluentAssertions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Entities.Base;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;
using Xunit;

namespace TaskManagement.Tests.Unit.Domain.Entities;

public class ProjectTests
{
    private readonly Faker<Project> _projectFaker;
    private readonly Faker<WorkItem> _workItemFaker;

    public ProjectTests()
    {
        _projectFaker = new Faker<Project>()
            .CustomInstantiator(f => new Project(
                Guid.NewGuid(),
                f.Commerce.ProductName(),
                f.Lorem.Paragraph(),
                Guid.NewGuid()));

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

    [Fact]
    public void Constructor_WhenCalled_ShouldCreateProjectWithProvidedValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Project";
        var description = "This is a test project";
        var ownerId = Guid.NewGuid();

        // Act
        var project = new Project(id, name, description, ownerId);

        // Assert
        project.Id.Should().Be(id);
        project.Name.Should().Be(name);
        project.Description.Should().Be(description);
        project.OwnerId.Should().Be(ownerId);
        project.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        project.WorkItems.Should().BeEmpty();
        project.Members.Should().BeEmpty();
    }

    [Fact]
    public void AddWorkItem_WhenProjectHasLessThan20Items_ShouldAddWorkItem()
    {
        // Arrange
        var project = _projectFaker.Generate();
        var workItem = _workItemFaker.Generate();

        // Act
        project.AddWorkItem(workItem);

        // Assert
        project.WorkItems.Should().Contain(workItem);
        project.WorkItems.Count.Should().Be(1);
    }

    [Fact]
    public void AddWorkItem_WhenProjectHas20Items_ShouldThrowDomainException()
    {
        // Arrange
        var project = _projectFaker.Generate();
        var workItems = _workItemFaker.Generate(20);

        foreach (var item in workItems)
        {
            project.AddWorkItem(item);
        }

        var newWorkItem = _workItemFaker.Generate();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => project.AddWorkItem(newWorkItem));
        exception.Message.Should().Be("Project cannot have more than 20 work items");
    }

    [Fact]
    public void RemoveWorkItem_WhenWorkItemIsCompleted_ShouldRemoveWorkItem()
    {
        // Arrange
        var project = _projectFaker.Generate();
        var workItem = _workItemFaker.Generate();

        // Set work item status to completed using reflection since we can't directly modify it
        typeof(WorkItem).GetProperty("Status")!
            .SetValue(workItem, WorkItemStatus.Completed);

        project.AddWorkItem(workItem);

        // Act
        project.RemoveWorkItem(workItem);

        // Assert
        project.WorkItems.Should().NotContain(workItem);
        project.WorkItems.Should().BeEmpty();
    }

    [Theory]
    [InlineData(WorkItemStatus.Pending)]
    [InlineData(WorkItemStatus.InProgress)]
    public void RemoveWorkItem_WhenWorkItemIsNotCompleted_ShouldThrowDomainException(WorkItemStatus status)
    {
        // Arrange
        var project = _projectFaker.Generate();
        var workItem = _workItemFaker.Generate();

        // Set work item status using reflection
        typeof(WorkItem).GetProperty("Status")!
            .SetValue(workItem, status);

        project.AddWorkItem(workItem);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => project.RemoveWorkItem(workItem));
        exception.Message.Should().Be("Cannot remove pending or in-progress work items");
    }

    [Fact]
    public void Project_WhenCreated_ShouldInheritFromEntityBase()
    {
        // Arrange
        var project = _projectFaker.Generate();

        // Act & Assert
        project.Should().BeAssignableTo<Entity<Guid>>();
    }

    [Fact]
    public void MarkAsUpdated_WhenCalled_ShouldUpdateUpdatedAtProperty()
    {
        // Arrange
        var project = _projectFaker.Generate();

        // Act
        project.MarkAsUpdated();

        // Assert
        project.UpdatedAt.Should().NotBeNull();
        project.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Equals_WhenComparingProjectsWithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var project1 = new Project(id, "Project 1", "Description 1", Guid.NewGuid());
        var project2 = new Project(id, "Project 2", "Description 2", Guid.NewGuid());

        // Act
        var result = project1.Equals(project2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WhenComparingProjectsWithDifferentIds_ShouldReturnFalse()
    {
        // Arrange
        var project1 = new Project(Guid.NewGuid(), "Project 1", "Description 1", Guid.NewGuid());
        var project2 = new Project(Guid.NewGuid(), "Project 1", "Description 1", Guid.NewGuid());

        // Act
        var result = project1.Equals(project2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EqualityOperator_WhenComparingProjectsWithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var project1 = new Project(id, "Project 1", "Description 1", Guid.NewGuid());
        var project2 = new Project(id, "Project 2", "Description 2", Guid.NewGuid());

        // Act
        var result = project1 == project2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void InequalityOperator_WhenComparingProjectsWithDifferentIds_ShouldReturnTrue()
    {
        // Arrange
        var project1 = new Project(Guid.NewGuid(), "Project 1", "Description 1", Guid.NewGuid());
        var project2 = new Project(Guid.NewGuid(), "Project 1", "Description 1", Guid.NewGuid());

        // Act
        var result = project1 != project2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_WhenCalledOnProjectsWithSameId_ShouldReturnSameValue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        var project1 = new Project(id, "Project 1", "Description 1", Guid.NewGuid());

        // Use reflection to set the same CreatedAt value for deterministic hash code
        typeof(Entity<Guid>).GetProperty("CreatedAt")!
            .SetValue(project1, createdAt);

        var project2 = new Project(id, "Project 2", "Description 2", Guid.NewGuid());

        typeof(Entity<Guid>).GetProperty("CreatedAt")!
            .SetValue(project2, createdAt);

        // Act
        var hashCode1 = project1.GetHashCode();
        var hashCode2 = project2.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }
}