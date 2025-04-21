using TaskManagement.Domain.Entities.Base;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities;

/// <summary>
/// Represents a unit of work within a project
/// </summary>
public class WorkItem : Entity<Guid>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime DueDate { get; private set; }
    public WorkItemStatus Status { get; private set; }
    public WorkItemPriority Priority { get; private init; }
    public Guid ProjectId { get; private set; }
    public Guid CreatedBy { get; private set; }

    private readonly List<WorkItemComment> _comments = new();
    public IReadOnlyCollection<WorkItemComment> Comments => _comments.AsReadOnly();

    private readonly List<WorkItemHistory> _history = new();
    public IReadOnlyCollection<WorkItemHistory> History => _history.AsReadOnly();

    private WorkItem()
    { }

    public WorkItem(
        Guid id,
        string title,
        string description,
        DateTime dueDate,
        WorkItemPriority priority,
        Guid projectId,
        Guid createdBy)
    {
        Id = id;
        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        ProjectId = projectId;
        CreatedBy = createdBy;
        Status = WorkItemStatus.Pending;

        RecordHistory("Work item created");
    }

    public void UpdateStatus(WorkItemStatus newStatus, Guid modifiedBy)
    {
        if (Status == newStatus) return;

        Status = newStatus;
        RecordHistory($"Status changed to {newStatus}", modifiedBy);
    }

    public void UpdateDetails(string title, string description, DateTime dueDate, Guid modifiedBy)
    {
        var changes = new List<string>();

        if (Title != title)
        {
            changes.Add($"Title changed from '{Title}' to '{title}'");
            Title = title;
        }

        if (Description != description)
        {
            changes.Add("Description updated");
            Description = description;
        }

        if (DueDate != dueDate)
        {
            changes.Add($"Due date changed from {DueDate:yyyy-MM-dd} to {dueDate:yyyy-MM-dd}");
            DueDate = dueDate;
        }

        if (changes.Any())
        {
            RecordHistory(string.Join(", ", changes), modifiedBy);
        }
    }

    public void AddComment(string content, Guid authorId)
    {
        var comment = new WorkItemComment(Guid.NewGuid(), content, authorId, Id);
        _comments.Add(comment);
        RecordHistory($"Comment added: {content}", authorId);
    }

    private void RecordHistory(string action, Guid? modifiedBy = null)
    {
        _history.Add(new WorkItemHistory(
            Guid.NewGuid(),
            action,
            DateTime.UtcNow,
            modifiedBy ?? CreatedBy,
            Id));
    }
}