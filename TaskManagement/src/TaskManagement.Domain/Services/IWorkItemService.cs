using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Services;

/// <summary>
/// Domain service for work item operations
/// </summary>
public interface IWorkItemService
{
    void ValidateWorkItemCreation(Project project, WorkItemPriority priority);
}