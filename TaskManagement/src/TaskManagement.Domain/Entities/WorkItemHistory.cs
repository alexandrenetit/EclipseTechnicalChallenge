using TaskManagement.Domain.Entities.Base;

namespace TaskManagement.Domain.Entities;

/// <summary>
/// Represents a historical record of changes to a work item
/// </summary>
public class WorkItemHistory : Entity<Guid>
{
    public string Action { get; private set; }
    public DateTime Timestamp { get; private set; }
    public Guid ModifiedBy { get; private set; }
    public Guid WorkItemId { get; private set; }

    public WorkItemHistory()
    { }

    public WorkItemHistory(Guid id, string action, DateTime timestamp, Guid modifiedBy, Guid workItemId)
    {
        Id = id;
        Action = action;
        Timestamp = timestamp;
        ModifiedBy = modifiedBy;
        WorkItemId = workItemId;
    }
}