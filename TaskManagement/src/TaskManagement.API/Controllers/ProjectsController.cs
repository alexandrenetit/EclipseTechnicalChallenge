using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers
{
    /// <summary>
    /// API controller for managing projects and their related operations
    /// </summary>
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        /// <summary>
        /// Initializes a new instance of the ProjectsController
        /// </summary>
        /// <param name="projectService">Project service for handling business logic</param>
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Retrieves all projects owned by a specific user
        /// </summary>
        /// <param name="ownerId">The unique identifier of the project owner</param>
        /// <returns>List of projects belonging to the specified owner</returns>
        /// <response code="200">Returns the list of projects</response>
        /// <response code="400">If the owner ID is invalid</response>
        [HttpGet("user/{ownerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetByOwner(Guid ownerId)
        {
            var projects = await _projectService.GetProjectsByOwnerAsync(ownerId);
            return Ok(projects);
        }

        /// <summary>
        /// Gets detailed information about a specific project including its work items
        /// </summary>
        /// <param name="id">The unique identifier of the project</param>
        /// <returns>The requested project details</returns>
        /// <response code="200">Returns the project details</response>
        /// <response code="400">If the project ID is invalid</response>
        /// <response code="404">If the project doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectDetailsResponse>> GetById(Guid id)
        {
            var project = await _projectService.GetProjectDetailsAsync(id);
            return Ok(project);
        }

        /// <summary>
        /// Creates a new project
        /// </summary>
        /// <param name="request">Project creation data</param>
        /// <returns>The newly created project</returns>
        /// <response code="201">Returns the newly created project</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="409">If project creation violates business rules</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProjectResponse>> Create(CreateProjectRequest request)
        {
            var project = await _projectService.CreateProjectAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }

        /// <summary>
        /// Deletes a specific project and all its associated work items
        /// </summary>
        /// <param name="id">The unique identifier of the project to delete</param>
        /// <returns>No content response</returns>
        /// <response code="204">If the project was successfully deleted</response>
        /// <response code="400">If the project ID is invalid</response>
        /// <response code="404">If the project doesn't exist</response>
        /// <response code="409">If project deletion violates business rules</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
    }
}