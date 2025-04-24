using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Services;

/// <summary>
/// Provides domain logic and validation rules for work item operations
/// </summary>
public interface IWorkItemServiceDomain
{
    /// <summary>
    /// Validates business rules for creating a new work item in a project
    /// </summary>
    /// <param name="project">The project where the work item will be created</param>
    /// <param name="priority">The priority level of the new work item</param>
    /// <exception cref="DomainException">
    /// Thrown when validation fails due to:
    /// - Project having maximum allowed work items (20)
    /// </exception>
    /// <remarks>
    /// Validation rules include:
    /// Project cannot contain more than 20 work items
    /// </remarks>
    void ValidateWorkItemCreation(Project project, WorkItemPriority priority);
}