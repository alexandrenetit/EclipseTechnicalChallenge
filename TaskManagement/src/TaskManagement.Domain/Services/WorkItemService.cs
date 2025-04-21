using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Services;

/// <summary>
/// Implementation of work item domain service
/// </summary>
public class WorkItemService : IWorkItemService
{
    public void ValidateWorkItemCreation(Project project, WorkItemPriority priority)
    {
        if (project.WorkItems.Count >= 20)
        {
            throw new DomainException("Project cannot have more than 20 work items");
        }

        if (priority == WorkItemPriority.High &&
            project.WorkItems.Count(i => i.Priority == WorkItemPriority.High) >= 5)
        {
            throw new DomainException("Project cannot have more than 5 high priority work items");
        }
    }
}