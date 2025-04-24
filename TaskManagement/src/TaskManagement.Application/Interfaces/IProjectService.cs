using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for project management operations in the application layer
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// Creates a new project with the specified details
        /// </summary>
        /// <param name="request">Contains project creation parameters including name, description, and owner ID</param>
        /// <returns>A ProjectResponse containing the created project's details</returns>
        /// <exception cref="ArgumentException">Thrown when request parameters are invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown when project creation fails due to business rules</exception>
        Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request);

        /// <summary>
        /// Retrieves all projects owned by a specific user
        /// </summary>
        /// <param name="ownerId">The unique identifier of the project owner</param>
        /// <returns>A collection of ProjectResponse objects representing the owner's projects</returns>
        /// <exception cref="ArgumentException">Thrown when ownerId is empty</exception>
        Task<IEnumerable<ProjectResponse>> GetProjectsByOwnerAsync(Guid ownerId);

        /// <summary>
        /// Retrieves detailed information about a specific project including its work items
        /// </summary>
        /// <param name="projectId">The unique identifier of the project</param>
        /// <returns>A ProjectDetailsResponse containing the project and its associated work items</returns>
        /// <exception cref="ArgumentException">Thrown when projectId is empty</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the specified project doesn't exist</exception>
        Task<ProjectDetailsResponse> GetProjectDetailsAsync(Guid projectId);

        /// <summary>
        /// Permanently deletes a project and all its associated work items
        /// </summary>
        /// <param name="projectId">The unique identifier of the project to delete</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <exception cref="ArgumentException">Thrown when projectId is empty</exception>
        /// <exception cref="InvalidOperationException">Thrown when deletion is not allowed</exception>
        Task DeleteProjectAsync(Guid projectId);
    }
}