using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.API.Controllers;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Tests.Integration.Endpoints;

/// <summary>
/// Integration tests for ProjectsController
/// Focus on testing the controller's behavior with mocked dependencies
/// Following Arrange-Act-Assert pattern for clear test structure
/// </summary>
public class ProjectsControllerIntegrationTests
{
    private readonly Mock<IProjectService> _mockProjectService;
    private readonly ProjectsController _controller;
    private readonly Faker _faker;

    public ProjectsControllerIntegrationTests()
    {
        _mockProjectService = new Mock<IProjectService>();
        _controller = new ProjectsController(_mockProjectService.Object);
        _faker = new Faker();
    }

    /// <summary>
    /// Test for GetByOwner method
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "GetByOwner should return OK with projects when valid owner ID is provided")]
    public async Task GetByOwner_WithValidOwnerId_ReturnsOkWithProjects()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var expectedProjects = GenerateProjectResponses(3, ownerId);

        _mockProjectService.Setup(service => service.GetProjectsByOwnerAsync(ownerId))
            .ReturnsAsync(expectedProjects);

        // Act
        var result = await _controller.GetByOwner(ownerId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<ProjectResponse>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedProjects = Assert.IsType<List<ProjectResponse>>(okResult.Value);
        Assert.Equal(expectedProjects.Count, returnedProjects.Count);
        _mockProjectService.Verify(service => service.GetProjectsByOwnerAsync(ownerId), Times.Once);
    }

    /// <summary>
    /// Test for GetByOwner method when no projects exist for the owner
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "GetByOwner should return empty list when no projects exist for owner")]
    public async Task GetByOwner_WithNoProjects_ReturnsEmptyList()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var emptyList = new List<ProjectResponse>();

        _mockProjectService.Setup(service => service.GetProjectsByOwnerAsync(ownerId))
            .ReturnsAsync(emptyList);

        // Act
        var result = await _controller.GetByOwner(ownerId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<ProjectResponse>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedProjects = Assert.IsType<List<ProjectResponse>>(okResult.Value);
        Assert.Empty(returnedProjects);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "GetById should return project details when valid project ID is provided")]
    public async Task GetById_WithValidId_ReturnsProjectDetails()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var expectedProject = GenerateProjectDetailsResponse(projectId);

        _mockProjectService.Setup(service => service.GetProjectDetailsAsync(projectId))
            .ReturnsAsync(expectedProject);

        // Act
        var result = await _controller.GetById(projectId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<ProjectDetailsResponse>>(result);

        if (actionResult.Result != null)
        {
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedProject = Assert.IsType<ProjectDetailsResponse>(okResult.Value);
            Assert.Equal(projectId, returnedProject.Id);
        }
        else
        {
            var returnedProject = Assert.IsType<ProjectDetailsResponse>(actionResult.Value);
            Assert.Equal(projectId, returnedProject.Id);
        }
    }

    /// <summary>
    /// Test for GetById method when invalid project ID is provided
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "GetById should return OK with null when invalid project ID is provided")]
    public async Task GetById_WithInvalidId_ReturnsOkWithNull()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        _mockProjectService.Setup(service => service.GetProjectDetailsAsync(invalidId))
            .ReturnsAsync((ProjectDetailsResponse)null);

        // Act
        var result = await _controller.GetById(invalidId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<ProjectDetailsResponse>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Null(okResult.Value);
    }

    /// <summary>
    /// Test for Create method
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Create should return CreatedAtAction with project when valid request is provided")]
    public async Task Create_WithValidRequest_ReturnsCreatedAtAction()
    {
        // Arrange
        var request = new CreateProjectRequest(
            _faker.Commerce.ProductName(),
            _faker.Lorem.Sentence(),
            Guid.NewGuid());

        var expectedProject = new ProjectResponse(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.OwnerId,
            0);

        _mockProjectService.Setup(service => service.CreateProjectAsync(request))
            .ReturnsAsync(expectedProject);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var actionResult = Assert.IsType<ActionResult<ProjectResponse>>(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);

        Assert.Equal(nameof(ProjectsController.GetById), createdAtActionResult.ActionName);
        Assert.Equal(expectedProject.Id, ((ProjectResponse)createdAtActionResult.Value).Id);
        _mockProjectService.Verify(service => service.CreateProjectAsync(request), Times.Once);
    }

    /// <summary>
    /// Test for Create method when invalid request is provided
    /// </summary>
    /// <returns></returns>
    [Fact(DisplayName = "Delete should return NoContent when valid project ID is provided")]
    public async Task Delete_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        _mockProjectService.Setup(service => service.DeleteProjectAsync(projectId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(projectId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockProjectService.Verify(service => service.DeleteProjectAsync(projectId), Times.Once);
    }

    private List<ProjectResponse> GenerateProjectResponses(int count, Guid ownerId)
    {
        var projects = new List<ProjectResponse>();

        for (int i = 0; i < count; i++)
        {
            projects.Add(new ProjectResponse(
                Guid.NewGuid(),
                _faker.Commerce.ProductName(),
                _faker.Lorem.Sentence(),
                ownerId,
                _faker.Random.Int(0, 10)));
        }

        return projects;
    }

    private ProjectDetailsResponse GenerateProjectDetailsResponse(Guid projectId)
    {
        var workItems = new List<WorkItemResponse>
        {
            new WorkItemResponse(
                Guid.NewGuid(),
                _faker.Commerce.ProductName(),
                _faker.Lorem.Sentence(),
                _faker.Date.Future(),
                WorkItemStatus.InProgress,
                WorkItemPriority.Medium,
                projectId,
                Guid.NewGuid())
        };

        return new ProjectDetailsResponse(
            projectId,
            _faker.Commerce.ProductName(),
            _faker.Lorem.Sentence(),
            Guid.NewGuid(),
            workItems);
    }
}