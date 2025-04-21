using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Mappers;

/// <summary>
/// Manual mapper for Project-related objects
/// </summary>
public static class ProjectMapper
{
    public static ProjectResponse ToResponse(this Project project)
    {
        return new ProjectResponse(
            project.Id,
            project.Name,
            project.Description,
            project.OwnerId,
            project.WorkItems.Count);
    }

    public static ProjectDetailsResponse ToDetailsResponse(this Project project)
    {
        return new ProjectDetailsResponse(
            project.Id,
            project.Name,
            project.Description,
            project.OwnerId,
            project.WorkItems.Select(wi => wi.ToResponse()));
    }
}