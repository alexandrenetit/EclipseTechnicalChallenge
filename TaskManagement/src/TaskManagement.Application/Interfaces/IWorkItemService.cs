using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Interfaces
{
    /// <summary>
    /// Defines operations for managing work items and their related entities
    /// </summary>
    public interface IWorkItemService
    {
        /// <summary>
        /// Creates a new work item in the specified project
        /// </summary>
        /// <param name="request">Contains work item creation parameters including title, description, due date, priority and project ID</param>
        /// <returns>WorkItemResponse containing the created work item details</returns>
        /// <exception cref="ArgumentException">Thrown when request parameters are invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown when project task limit is exceeded or priority rules are violated</exception>
        Task<WorkItemResponse> CreateWorkItemAsync(CreateWorkItemRequest request);

        /// <summary>
        /// Retrieves complete details of a work item including comments and history
        /// </summary>
        /// <param name="workItemId">The unique identifier of the work item</param>
        /// <returns>WorkItemDetailsResponse with full work item information</returns>
        /// <exception cref="ArgumentException">Thrown when workItemId is empty</exception>
        /// <exception cref="KeyNotFoundException">Thrown when work item doesn't exist</exception>
        Task<WorkItemDetailsResponse> GetWorkItemDetailsAsync(Guid workItemId);

        /// <summary>
        /// Updates an existing work item's properties
        /// </summary>
        /// <param name="workItemId">The ID of the work item to update</param>
        /// <param name="request">Contains updated work item properties</param>
        /// <param name="modifiedBy">ID of the user making the changes</param>
        /// <returns>Updated WorkItemResponse</returns>
        /// <exception cref="ArgumentException">Thrown when parameters are invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown when update violates business rules</exception>
        Task<WorkItemResponse> UpdateWorkItemAsync(Guid workItemId, UpdateWorkItemRequest request, Guid modifiedBy);

        /// <summary>
        /// Permanently deletes a work item and its associated comments
        /// </summary>
        /// <param name="workItemId">The ID of the work item to delete</param>
        /// <returns>Task representing the asynchronous operation</returns>
        /// <exception cref="ArgumentException">Thrown when workItemId is empty</exception>
        /// <exception cref="InvalidOperationException">Thrown when work item is in a state that prevents deletion</exception>
        Task DeleteWorkItemAsync(Guid workItemId);

        /// <summary>
        /// Adds a new comment to an existing work item
        /// </summary>
        /// <param name="workItemId">The ID of the work item to comment on</param>
        /// <param name="request">Contains comment content and author information</param>
        /// <returns>CommentResponse with the created comment details</returns>
        /// <exception cref="ArgumentException">Thrown when parameters are invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown when work item is closed or archived</exception>
        Task<CommentResponse> AddCommentAsync(Guid workItemId, AddCommentRequest request);
    }
}