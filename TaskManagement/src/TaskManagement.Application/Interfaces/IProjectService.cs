using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Interfaces
{
    /// <summary>
    /// Interface for project service operations
    /// </summary>
    public interface IProjectService
    {
        Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request);

        Task<IEnumerable<ProjectResponse>> GetProjectsByOwnerAsync(Guid ownerId);

        Task<ProjectDetailsResponse> GetProjectDetailsAsync(Guid projectId);

        Task DeleteProjectAsync(Guid projectId);
    }
}