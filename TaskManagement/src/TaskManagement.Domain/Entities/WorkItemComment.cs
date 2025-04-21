using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities.Base;

namespace TaskManagement.Domain.Entities;

/// <summary>
/// Represents a comment on a work item
/// </summary>
public class WorkItemComment : Entity<Guid>
{
    public string Content { get; private set; }
    public Guid AuthorId { get; private set; }
    public Guid WorkItemId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public WorkItemComment() { }

    public WorkItemComment(Guid id, string content, Guid authorId, Guid workItemId)
    {
        Id = id;
        Content = content;
        AuthorId = authorId;
        WorkItemId = workItemId;
        CreatedAt = DateTime.UtcNow;
    }
}
