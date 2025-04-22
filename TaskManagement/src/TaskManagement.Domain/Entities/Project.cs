using TaskManagement.Domain.Entities.Base;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Entities;

/// <summary>
/// Represents a project containing multiple work items
/// </summary>
public class Project : Entity<Guid>
{
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public Guid OwnerId { get; private set; }

    public virtual User Owner { get; private set; }

    private readonly List<WorkItem> _workItems = new();

    public IReadOnlyCollection<WorkItem> WorkItems => _workItems.AsReadOnly();

    private readonly List<ProjectMember> _members = new();

    public IReadOnlyCollection<ProjectMember> Members => _members.AsReadOnly();   

    private Project()
    { }

    public Project(Guid id, string name, string description, Guid ownerId)
    {
        Id = id;
        Name = name;
        Description = description;
        OwnerId = ownerId;
    }

    public void AddWorkItem(WorkItem workItem)
    {
        if (_workItems.Count >= 20)
            throw new DomainException("Project cannot have more than 20 work items");

        _workItems.Add(workItem);
    }

    public void RemoveWorkItem(WorkItem workItem)
    {
        if (workItem.Status != WorkItemStatus.Completed)
            throw new DomainException("Cannot remove pending or in-progress work items");

        _workItems.Remove(workItem);
    }
}