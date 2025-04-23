using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.API.Controllers;
using Xunit;
using Bogus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;

namespace TaskManagement.API.Tests.Integration
{
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

            // For ActionResult<T>, the actual result is in the .Result property when returning IActionResult
            // or in the .Value property when returning T directly
            if (actionResult.Result != null)
            {
                var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
                var returnedProject = Assert.IsType<ProjectDetailsResponse>(okResult.Value);
                Assert.Equal(projectId, returnedProject.Id);
            }
            else
            {
                // This path would be taken if the controller returned the value directly
                var returnedProject = Assert.IsType<ProjectDetailsResponse>(actionResult.Value);
                Assert.Equal(projectId, returnedProject.Id);
            }
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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
}