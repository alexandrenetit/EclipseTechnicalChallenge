using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Mappers;

public static class WorkItemMapper
{
    public static WorkItemResponse ToResponse(this WorkItem workItem)
    {
        return new WorkItemResponse(
            workItem.Id,
            workItem.Title,
            workItem.Description,
            workItem.DueDate,
            workItem.Status,
            workItem.Priority,
            workItem.ProjectId,
            workItem.CreatedBy);
    }

    public static WorkItemDetailsResponse ToDetailsResponse(this WorkItem workItem)
    {
        return new WorkItemDetailsResponse(
            workItem.Id,
            workItem.Title,
            workItem.Description,
            workItem.DueDate,
            workItem.Status,
            workItem.Priority,
            workItem.ProjectId,
            workItem.CreatedBy,
            workItem.Comments.Select(c => new CommentResponse(
                c.Id,
                c.Content,
                c.AuthorId,
                c.CreatedAt)),
            workItem.History.Select(h => new HistoryResponse(
                h.Id,
                h.Action,
                h.Timestamp,
                h.ModifiedBy)));
    }

    public static CommentResponse ToResponse(this WorkItemComment comment)
    {
        return new CommentResponse(
            comment.Id,
            comment.Content,
            comment.AuthorId,
            comment.CreatedAt);
    }
}