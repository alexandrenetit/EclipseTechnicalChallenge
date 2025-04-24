using Bogus;
using Moq;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repositories;
using Xunit;

namespace TaskManagement.Tests.Unit.Application.Services;

/// <summary>
/// Tests for the ProjectService class that handles project-related operations
/// </summary>
public class ProjectServiceTests
{
    private readonly Faker _faker;
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly ProjectService _projectService;

    public ProjectServiceTests()
    {
        _faker = new Faker();
        _mockProjectRepository = new Mock<IProjectRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _projectService = new ProjectService(_mockProjectRepository.Object, _mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests that creating a project with valid request data succeeds and returns the created project
    /// </summary>
    [Fact(DisplayName = "CreateProjectAsync should create and return project when request is valid")]
    public async Task CreateProjectAsync_WithValidRequest_ShouldCreateAndReturnProject()
    {
        // Arrange
        var request = new CreateProjectRequest(
            Name: _faker.Company.CompanyName(),
            Description: _faker.Lorem.Paragraph(),
            OwnerId: Guid.NewGuid()
        );

        Project capturedProject = null;
        _mockProjectRepository
            .Setup(repo => repo.AddAsync(It.IsAny<Project>()))
            .Callback<Project>(project => capturedProject = project)
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _projectService.CreateProjectAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.OwnerId, result.OwnerId);

        _mockProjectRepository.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);

        // Verify project was created with correct values
        Assert.NotNull(capturedProject);
        Assert.Equal(request.Name, capturedProject.Name);
        Assert.Equal(request.Description, capturedProject.Description);
        Assert.Equal(request.OwnerId, capturedProject.OwnerId);
    }

    /// <summary>
    /// Tests that creating a project with invalid data throws validation exceptions with appropriate error messages
    /// </summary>
    [Theory(DisplayName = "CreateProjectAsync should throw validation exception when request is invalid")]
    [InlineData("", "Description", "Name is required")]
    [InlineData("Name that is way too long and exceeds the maximum allowed length of one hundred characters which will trigger validation errors", "Description", "name must not exceed")]
    [InlineData("Valid Name", "Description that is extremely long and definitely exceeds the maximum allowed length of five hundred characters. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. And we need even more text to exceed the limit.", "description must not exceed")]
    public async Task CreateProjectAsync_WithInvalidRequest_ShouldThrowValidationException(string name, string description, string expectedErrorSubstring)
    {
        // Arrange
        var request = new CreateProjectRequest(
            Name: name,
            Description: description,
            OwnerId: Guid.NewGuid()
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => _projectService.CreateProjectAsync(request)
        );

        Assert.Contains(
            exception.Errors,
            error => error.ErrorMessage.ToLower().Contains(expectedErrorSubstring.ToLower())
        );

        // Verify repository methods were never called
        _mockProjectRepository.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    /// <summary>
    /// Tests that creating a project with empty owner ID throws a validation exception
    /// </summary>
    [Fact(DisplayName = "CreateProjectAsync should throw validation exception when owner ID is empty")]
    public async Task CreateProjectAsync_WithEmptyOwnerId_ShouldThrowValidationException()
    {
        // Arrange
        var request = new CreateProjectRequest(
            Name: _faker.Company.CompanyName(),
            Description: _faker.Lorem.Paragraph(),
            OwnerId: Guid.Empty
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => _projectService.CreateProjectAsync(request)
        );

        Assert.Contains(
            exception.Errors,
            error => error.ErrorMessage.ToLower().Contains("owner id must be a valid guid")
        );

        // Verify repository methods were never called
        _mockProjectRepository.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    /// <summary>
    /// Tests that retrieving projects by owner ID returns all projects for that owner
    /// </summary>
    [Fact(DisplayName = "GetProjectsByOwnerAsync should return all projects for the specified owner")]
    public async Task GetProjectsByOwnerAsync_WithExistingProjects_ShouldReturnProjects()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var projects = new List<Project>
        {
            CreateTestProject(ownerId),
            CreateTestProject(ownerId),
            CreateTestProject(ownerId)
        };

        _mockProjectRepository
            .Setup(repo => repo.GetByOwnerIdAsync(ownerId))
            .ReturnsAsync(projects);

        // Act
        var result = await _projectService.GetProjectsByOwnerAsync(ownerId);

        // Assert
        var projectResponses = result.ToList();
        Assert.Equal(projects.Count, projectResponses.Count);

        for (int i = 0; i < projects.Count; i++)
        {
            Assert.Equal(projects[i].Id, projectResponses[i].Id);
            Assert.Equal(projects[i].Name, projectResponses[i].Name);
            Assert.Equal(projects[i].Description, projectResponses[i].Description);
            Assert.Equal(projects[i].OwnerId, projectResponses[i].OwnerId);
        }

        _mockProjectRepository.Verify(repo => repo.GetByOwnerIdAsync(ownerId), Times.Once);
    }

    /// <summary>
    /// Tests that retrieving projects for an owner with no projects returns an empty list
    /// </summary>
    [Fact(DisplayName = "GetProjectsByOwnerAsync should return empty list when owner has no projects")]
    public async Task GetProjectsByOwnerAsync_WithNoProjects_ShouldReturnEmptyList()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var emptyList = new List<Project>();

        _mockProjectRepository
            .Setup(repo => repo.GetByOwnerIdAsync(ownerId))
            .ReturnsAsync(emptyList);

        // Act
        var result = await _projectService.GetProjectsByOwnerAsync(ownerId);

        // Assert
        Assert.Empty(result);
        _mockProjectRepository.Verify(repo => repo.GetByOwnerIdAsync(ownerId), Times.Once);
    }

    /// <summary>
    /// Tests that retrieving project details for an existing project returns the project with work items
    /// </summary>
    [Fact(DisplayName = "GetProjectDetailsAsync should return project details including work items when project exists")]
    public async Task GetProjectDetailsAsync_WithExistingProject_ShouldReturnProjectDetails()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var project = CreateTestProject(ownerId, projectId);

        // Add some work items to the project
        var workItem1 = CreateTestWorkItem(project.Id);
        var workItem2 = CreateTestWorkItem(project.Id);

        // Add work items to the project using reflection
        var workItemsField = typeof(Project).GetField("_workItems",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var workItems = (List<WorkItem>)workItemsField.GetValue(project);
        workItems.Add(workItem1);
        workItems.Add(workItem2);

        _mockProjectRepository
            .Setup(repo => repo.GetWithWorkItemsAsync(projectId))
            .ReturnsAsync(project);

        // Act
        var result = await _projectService.GetProjectDetailsAsync(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(project.Id, result.Id);
        Assert.Equal(project.Name, result.Name);
        Assert.Equal(project.Description, result.Description);
        Assert.Equal(project.OwnerId, result.OwnerId);

        var workItemResponses = result.WorkItems.ToList();
        Assert.Equal(2, workItemResponses.Count);
        Assert.Equal(workItem1.Id, workItemResponses[0].Id);
        Assert.Equal(workItem2.Id, workItemResponses[1].Id);

        _mockProjectRepository.Verify(repo => repo.GetWithWorkItemsAsync(projectId), Times.Once);
    }

    /// <summary>
    /// Tests that retrieving project details for a non-existing project throws KeyNotFoundException
    /// </summary>
    [Fact(DisplayName = "GetProjectDetailsAsync should throw KeyNotFoundException when project does not exist")]
    public async Task GetProjectDetailsAsync_WithNonExistingProject_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        _mockProjectRepository
            .Setup(repo => repo.GetWithWorkItemsAsync(projectId))
            .ReturnsAsync((Project)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _projectService.GetProjectDetailsAsync(projectId)
        );

        _mockProjectRepository.Verify(repo => repo.GetWithWorkItemsAsync(projectId), Times.Once);
    }

    /// <summary>
    /// Tests that deleting a project with all completed work items succeeds
    /// </summary>
    [Fact(DisplayName = "DeleteProjectAsync should delete project when all work items are completed")]
    public async Task DeleteProjectAsync_WithProjectHavingAllCompletedWorkItems_ShouldDeleteProject()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var project = CreateTestProject(ownerId, projectId);

        // Add some completed work items to the project
        var workItem1 = CreateTestWorkItem(project.Id, WorkItemStatus.Completed);
        var workItem2 = CreateTestWorkItem(project.Id, WorkItemStatus.Completed);

        // Add work items to the project using reflection
        var workItemsField = typeof(Project).GetField("_workItems",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var workItems = (List<WorkItem>)workItemsField.GetValue(project);
        workItems.Add(workItem1);
        workItems.Add(workItem2);

        _mockProjectRepository
            .Setup(repo => repo.GetWithWorkItemsAsync(projectId))
            .ReturnsAsync(project);

        _mockProjectRepository
            .Setup(repo => repo.DeleteAsync(project))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(1);

        // Act
        await _projectService.DeleteProjectAsync(projectId);

        // Assert
        _mockProjectRepository.Verify(repo => repo.GetWithWorkItemsAsync(projectId), Times.Once);
        _mockProjectRepository.Verify(repo => repo.DeleteAsync(project), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    /// <summary>
    /// Tests that deleting a project with pending work items throws InvalidOperationException
    /// </summary>
    [Fact(DisplayName = "DeleteProjectAsync should throw InvalidOperationException when project has pending work items")]
    public async Task DeleteProjectAsync_WithProjectHavingPendingWorkItems_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var project = CreateTestProject(ownerId, projectId);

        // Add mixed work items to the project (some completed, some pending)
        var workItem1 = CreateTestWorkItem(project.Id, WorkItemStatus.Completed);
        var workItem2 = CreateTestWorkItem(project.Id, WorkItemStatus.Pending);

        // Add work items to the project using reflection
        var workItemsField = typeof(Project).GetField("_workItems",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var workItems = (List<WorkItem>)workItemsField.GetValue(project);
        workItems.Add(workItem1);
        workItems.Add(workItem2);

        _mockProjectRepository
            .Setup(repo => repo.GetWithWorkItemsAsync(projectId))
            .ReturnsAsync(project);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _projectService.DeleteProjectAsync(projectId)
        );

        Assert.Contains("Cannot delete project with pending or in-progress work items", exception.Message);

        _mockProjectRepository.Verify(repo => repo.GetWithWorkItemsAsync(projectId), Times.Once);
        _mockProjectRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Project>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    /// <summary>
    /// Tests that deleting a non-existing project throws KeyNotFoundException
    /// </summary>
    [Fact(DisplayName = "DeleteProjectAsync should throw KeyNotFoundException when project does not exist")]
    public async Task DeleteProjectAsync_WithNonExistingProject_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        _mockProjectRepository
            .Setup(repo => repo.GetWithWorkItemsAsync(projectId))
            .ReturnsAsync((Project)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _projectService.DeleteProjectAsync(projectId)
        );

        _mockProjectRepository.Verify(repo => repo.GetWithWorkItemsAsync(projectId), Times.Once);
        _mockProjectRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Project>()), Times.Never);
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

    private WorkItem CreateTestWorkItem(Guid projectId, WorkItemStatus status = WorkItemStatus.Pending)
    {
        var workItem = new WorkItem(
            Guid.NewGuid(),
            _faker.Lorem.Sentence(),
            _faker.Lorem.Paragraph(),
            _faker.Date.Future(),
            GetRandomPriority(),
            projectId,
            Guid.NewGuid()
        );

        // If we need completed status, use reflection to change it
        if (status == WorkItemStatus.Completed)
        {
            var statusProperty = typeof(WorkItem).GetProperty("Status");
            statusProperty.SetValue(workItem, WorkItemStatus.Completed);
        }

        return workItem;
    }

    private WorkItemPriority GetRandomPriority()
    {
        var priorities = Enum.GetValues(typeof(WorkItemPriority));
        return (WorkItemPriority)priorities.GetValue(_faker.Random.Int(0, priorities.Length - 1));
    }
}