using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("user/{ownerId}")]
        public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetByOwner(Guid ownerId)
        {
            var projects = await _projectService.GetProjectsByOwnerAsync(ownerId);
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDetailsResponse>> GetById(Guid id)
        {
            var project = await _projectService.GetProjectDetailsAsync(id);
            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResponse>> Create(CreateProjectRequest request)
        {
            var project = await _projectService.CreateProjectAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
    }
}