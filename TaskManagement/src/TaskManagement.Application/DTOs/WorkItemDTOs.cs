using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs;

public record CreateWorkItemRequest(string Title, string Description, DateTime DueDate,
    WorkItemPriority Priority, Guid ProjectId, Guid CreatedBy);
public record UpdateWorkItemRequest(string? Title, string? Description, DateTime? DueDate, 
    WorkItemStatus? Status);
public record WorkItemResponse(Guid Id, string Title, string Description, DateTime DueDate,
    WorkItemStatus Status, WorkItemPriority Priority, Guid ProjectId, Guid CreatedBy);
public record WorkItemDetailsResponse(Guid Id, string Title, string Description, DateTime DueDate, 
    WorkItemStatus Status, WorkItemPriority Priority, Guid ProjectId, Guid CreatedBy, 
    IEnumerable<CommentResponse> Comments, IEnumerable<HistoryResponse> History);
public record CommentResponse(Guid Id, string Content, Guid AuthorId, DateTime CreatedAt);
public record HistoryResponse(Guid Id, string Action, DateTime Timestamp, Guid ModifiedBy);
public record AddCommentRequest(string Content, Guid AuthorId);