using TaskManagement.Application.DTOs;

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