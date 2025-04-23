using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Infrastructure.Repositories;
using Xunit;

namespace TaskManagement.Tests.Unit.Infrastructure.Repositories;

public class ProjectRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldAddProjectToContext()
    {
        // Arrange
        var dbContext = CreateDbContext("AddAsync_Test");
        var repository = new ProjectRepository(dbContext);
        var project = new Project(Guid.NewGuid(), "Test Project", "Description", Guid.NewGuid());

        // Act
        await repository.AddAsync(project);
        await dbContext.SaveChangesAsync();

        // Assert
        dbContext.Projects.Should().Contain(p => p.Id == project.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProjectExists_ShouldReturnProject()
    {
        // Arrange
        var dbContext = CreateDbContext("GetByIdAsync_Exists_Test");
        var repository = new ProjectRepository(dbContext);
        var projectId = Guid.NewGuid();
        var project = new Project(projectId, "Test Project", "Description", Guid.NewGuid());

        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(projectId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(projectId);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProjectDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var dbContext = CreateDbContext("GetByIdAsync_NotExists_Test");
        var repository = new ProjectRepository(dbContext);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProjects()
    {
        // Arrange
        var dbContext = CreateDbContext("GetAllAsync_Test");
        var repository = new ProjectRepository(dbContext);

        // Add three test projects
        var projects = new List<Project>
        {
            new Project(Guid.NewGuid(), "Project 1", "Description 1", Guid.NewGuid()),
            new Project(Guid.NewGuid(), "Project 2", "Description 2", Guid.NewGuid()),
            new Project(Guid.NewGuid(), "Project 3", "Description 3", Guid.NewGuid())
        };

        dbContext.Projects.AddRange(projects);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(3);
        result.Select(p => p.Id).Should().Contain(projects.Select(p => p.Id));
    }

    [Fact]
    public async Task GetByOwnerIdAsync_ShouldReturnOnlyProjectsWithMatchingOwnerId()
    {
        // Arrange
        var dbContext = CreateDbContext("GetByOwnerIdAsync_Test");
        var repository = new ProjectRepository(dbContext);
        var ownerId = Guid.NewGuid();
        var otherId = Guid.NewGuid();

        // Add projects with different owner IDs
        var ownerProjects = new List<Project>
        {
            new Project(Guid.NewGuid(), "Owner Project 1", "Description 1", ownerId),
            new Project(Guid.NewGuid(), "Owner Project 2", "Description 2", ownerId)
        };

        var otherProjects = new List<Project>
        {
            new Project(Guid.NewGuid(), "Other Project", "Description", otherId)
        };

        dbContext.Projects.AddRange(ownerProjects);
        dbContext.Projects.AddRange(otherProjects);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetByOwnerIdAsync(ownerId);

        // Assert
        result.Should().HaveCount(2);
        result.All(p => p.OwnerId == ownerId).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProjectInContext()
    {
        // Arrange
        var dbContext = CreateDbContext("UpdateAsync_Test");
        var repository = new ProjectRepository(dbContext);
        var projectId = Guid.NewGuid();

        // Create and add initial project
        var project = new Project(projectId, "Initial Name", "Initial Description", Guid.NewGuid());
        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync();

        // Modify the project using reflection (since properties have private setters)
        typeof(Project).GetProperty("Name")!.SetValue(project, "Updated Name");
        typeof(Project).GetProperty("Description")!.SetValue(project, "Updated Description");

        // Act
        await repository.UpdateAsync(project);
        await dbContext.SaveChangesAsync();

        // Assert - Get fresh instance from DB to verify changes
        var updatedProject = await dbContext.Projects.FindAsync(projectId);
        updatedProject.Should().NotBeNull();
        updatedProject!.Name.Should().Be("Updated Name");
        updatedProject.Description.Should().Be("Updated Description");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveProjectFromContext()
    {
        // Arrange
        var dbContext = CreateDbContext("DeleteAsync_Test");
        var repository = new ProjectRepository(dbContext);
        var projectId = Guid.NewGuid();
        var project = new Project(projectId, "Test Project", "Description", Guid.NewGuid());

        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(project);
        await dbContext.SaveChangesAsync();

        // Assert
        var deletedProject = await dbContext.Projects.FindAsync(projectId);
        deletedProject.Should().BeNull();
    }

    [Fact]
    public async Task GetWithWorkItemsAsync_ShouldIncludeWorkItems()
    {
        // Arrange
        var dbContext = CreateDbContext("GetWithWorkItemsAsync_Test");
        var repository = new ProjectRepository(dbContext);
        var projectId = Guid.NewGuid();
        var project = new Project(projectId, "Test Project", "Description", Guid.NewGuid());

        // Add project to context
        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync();

        // Add work items to project
        var workItem1 = new WorkItem(
            Guid.NewGuid(),
            "Work Item 1",
            "Description 1",
            DateTime.UtcNow.AddDays(1),
            WorkItemPriority.Low,
            projectId,
            Guid.NewGuid());

        var workItem2 = new WorkItem(
            Guid.NewGuid(),
            "Work Item 2",
            "Description 2",
            DateTime.UtcNow.AddDays(2),
            WorkItemPriority.Medium,
            projectId,
            Guid.NewGuid());

        project.AddWorkItem(workItem1);
        project.AddWorkItem(workItem2);

        dbContext.WorkItems.AddRange(workItem1, workItem2);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetWithWorkItemsAsync(projectId);

        // Assert
        result.Should().NotBeNull();
        result!.WorkItems.Should().HaveCount(2);
        result.WorkItems.Select(w => w.Id).Should().Contain(new[] { workItem1.Id, workItem2.Id });
    }

    private ApplicationDbContext CreateDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new ApplicationDbContext(options);
    }
}