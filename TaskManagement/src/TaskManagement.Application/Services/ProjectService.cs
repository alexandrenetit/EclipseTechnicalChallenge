using FluentValidation;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mappers;
using TaskManagement.Application.Validators;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Services;

/// <summary>
/// Application service for project operations
/// </summary>
public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProjectService(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request)
    {
        var validator = new CreateProjectRequestValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var project = new Project(Guid.NewGuid(), request.Name, request.Description, request.OwnerId);

        await _projectRepository.AddAsync(project);
        await _unitOfWork.CommitAsync();

        return project.ToResponse();
    }

    public async Task<IEnumerable<ProjectResponse>> GetProjectsByOwnerAsync(Guid ownerId)
    {
        var projects = await _projectRepository.GetByOwnerIdAsync(ownerId);
        return projects.Select(p => p.ToResponse());
    }

    public async Task<ProjectDetailsResponse> GetProjectDetailsAsync(Guid projectId)
    {
        var project = await _projectRepository.GetWithWorkItemsAsync(projectId);
        if (project == null)
        {
            throw new KeyNotFoundException("Project not found");
        }

        return project.ToDetailsResponse();
    }

    public async Task DeleteProjectAsync(Guid projectId)
    {
        var project = await _projectRepository.GetWithWorkItemsAsync(projectId);
        if (project == null)
        {
            throw new KeyNotFoundException("Project not found");
        }

        if (project.WorkItems.Any(wi => wi.Status != WorkItemStatus.Completed))
        {
            throw new InvalidOperationException(
                "Cannot delete project with pending or in-progress work items. " +
                "Please complete or remove all work items first.");
        }

        await _projectRepository.DeleteAsync(project);
        await _unitOfWork.CommitAsync();
    }
}