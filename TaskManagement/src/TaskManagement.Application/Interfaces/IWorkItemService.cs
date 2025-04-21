using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Interfaces
{
    public interface IWorkItemService
    {
        Task<WorkItemResponse> CreateWorkItemAsync(CreateWorkItemRequest request);
        Task<WorkItemDetailsResponse> GetWorkItemDetailsAsync(Guid workItemId);
        Task<WorkItemResponse> UpdateWorkItemAsync(Guid workItemId, UpdateWorkItemRequest request, Guid modifiedBy);
        Task DeleteWorkItemAsync(Guid workItemId);
        Task<CommentResponse> AddCommentAsync(Guid workItemId, AddCommentRequest request);        
    }
}
