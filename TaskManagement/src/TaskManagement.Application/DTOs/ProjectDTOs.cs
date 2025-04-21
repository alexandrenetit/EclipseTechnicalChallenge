namespace TaskManagement.Application.DTOs;

/// <summary>
/// Data transfer objects for Project operations
/// </summary>
public record CreateProjectRequest(string Name, string Description, Guid OwnerId);
public record ProjectResponse(Guid Id, string Name, string Description, Guid OwnerId,
    int WorkItemCount);
public record ProjectDetailsResponse(Guid Id, string Name, string Description,
    Guid OwnerId, IEnumerable<WorkItemResponse> WorkItems);