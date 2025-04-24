using FluentAssertions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Entities.Base;
using Xunit;

namespace TaskManagement.Tests.Unit.Domain.Entities;

/// <summary>
/// Tests for the ProjectMember entity
/// </summary>
public class ProjectMemberTests
{
    /// <summary>
    /// Tests that constructor creates project member with correct values
    /// </summary>
    [Fact(DisplayName = "Constructor should create project member with provided values")]
    public void Constructor_WhenCalled_ShouldCreateProjectMemberWithProvidedValues()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        var projectMember = new ProjectMember(projectId, userId);

        // Assert
        projectMember.ProjectId.Should().Be(projectId);
        projectMember.UserId.Should().Be(userId);
        projectMember.JoinedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        projectMember.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that ProjectMember inherits from EntityBase
    /// </summary>
    [Fact(DisplayName = "ProjectMember should inherit from EntityBase")]
    public void ProjectMember_WhenCreated_ShouldInheritFromEntityBase()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        var projectMember = new ProjectMember(projectId, userId);

        // Assert
        projectMember.Should().BeAssignableTo<Entity<Guid>>();
    }

    /// <summary>
    /// Tests that MarkAsUpdated updates the UpdatedAt property
    /// </summary>
    [Fact(DisplayName = "MarkAsUpdated should update UpdatedAt property")]
    public void MarkAsUpdated_WhenCalled_ShouldUpdateUpdatedAtProperty()
    {
        // Arrange
        var projectMember = new ProjectMember(Guid.NewGuid(), Guid.NewGuid());

        // Act
        projectMember.MarkAsUpdated();

        // Assert
        projectMember.UpdatedAt.Should().NotBeNull();
        projectMember.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that Equals returns true for ProjectMembers with same ID
    /// </summary>
    [Fact(DisplayName = "Equals should return true for ProjectMembers with same ID")]
    public void Equals_WhenComparingProjectMembersWithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Use reflection to set the Id since it has a protected setter
        var projectMember1 = new ProjectMember(Guid.NewGuid(), Guid.NewGuid());
        typeof(Entity<Guid>).GetProperty("Id")!.SetValue(projectMember1, id);

        var projectMember2 = new ProjectMember(Guid.NewGuid(), Guid.NewGuid());
        typeof(Entity<Guid>).GetProperty("Id")!.SetValue(projectMember2, id);

        // Act
        var result = projectMember1.Equals(projectMember2);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that Equals returns false for ProjectMembers with different IDs
    /// </summary>
    [Fact(DisplayName = "Equals should return false for ProjectMembers with different IDs")]
    public void Equals_WhenComparingProjectMembersWithDifferentIds_ShouldReturnFalse()
    {
        // Arrange
        var projectMember1 = new ProjectMember(Guid.NewGuid(), Guid.NewGuid());
        typeof(Entity<Guid>).GetProperty("Id")!.SetValue(projectMember1, Guid.NewGuid());

        var projectMember2 = new ProjectMember(Guid.NewGuid(), Guid.NewGuid());
        typeof(Entity<Guid>).GetProperty("Id")!.SetValue(projectMember2, Guid.NewGuid());

        // Act
        var result = projectMember1.Equals(projectMember2);

        // Assert
        result.Should().BeFalse();
    }
}