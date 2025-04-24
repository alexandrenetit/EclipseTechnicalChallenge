using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers
{
    /// <summary>
    /// API controller for managing work items and their related operations
    /// </summary>
    [ApiController]
    [Route("api/workitems")]
    public class WorkItemsController : ControllerBase
    {
        private readonly IWorkItemService _workItemService;

        /// <summary>
        /// Initializes a new instance of the WorkItemsController
        /// </summary>
        /// <param name="workItemService">Service for work item operations</param>
        public WorkItemsController(IWorkItemService workItemService)
        {
            _workItemService = workItemService;
        }

        /// <summary>
        /// Retrieves detailed information about a specific work item
        /// </summary>
        /// <param name="id">The unique identifier of the work item</param>
        /// <returns>Complete work item details including comments and history</returns>
        /// <response code="200">Returns the requested work item details</response>
        /// <response code="400">If the ID is invalid</response>
        /// <response code="404">If the work item doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkItemDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkItemDetailsResponse>> GetById(Guid id)
        {
            var workItem = await _workItemService.GetWorkItemDetailsAsync(id);
            return Ok(workItem);
        }

        /// <summary>
        /// Creates a new work item
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/workitems
        ///     {
        ///         "title": "Implement authentication",
        ///         "description": "Add JWT authentication to API",
        ///         "dueDate": "2023-12-31",
        ///         "priority": 2,
        ///         "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        /// </remarks>
        /// <param name="request">Work item creation data</param>
        /// <returns>The newly created work item</returns>
        /// <response code="201">Returns the newly created work item</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="409">If creation violates business rules (e.g., project task limits)</response>
        [HttpPost]
        [ProducesResponseType(typeof(WorkItemResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<WorkItemResponse>> Create(CreateWorkItemRequest request)
        {
            var workItem = await _workItemService.CreateWorkItemAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = workItem.Id }, workItem);
        }

        /// <summary>
        /// Updates an existing work item
        /// </summary>
        /// <param name="id">The ID of the work item to update</param>
        /// <param name="request">Updated work item data</param>
        /// <param name="modifiedBy">ID of the user making changes (from header)</param>
        /// <returns>The updated work item</returns>
        /// <response code="200">Returns the updated work item</response>
        /// <response code="400">If parameters are invalid</response>
        /// <response code="404">If the work item doesn't exist</response>
        /// <response code="409">If update violates business rules</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(WorkItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<WorkItemResponse>> Update(Guid id, UpdateWorkItemRequest request, [FromHeader] Guid modifiedBy)
        {
            var workItem = await _workItemService.UpdateWorkItemAsync(id, request, modifiedBy);
            return Ok(workItem);
        }

        /// <summary>
        /// Deletes a work item
        /// </summary>
        /// <param name="id">The ID of the work item to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">If deletion was successful</response>
        /// <response code="400">If the ID is invalid</response>
        /// <response code="404">If the work item doesn't exist</response>
        /// <response code="409">If deletion violates business rules</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _workItemService.DeleteWorkItemAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Adds a comment to a work item
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/workitems/{id}/comments
        ///     {
        ///         "content": "This needs more clarification",
        ///         "authorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        /// </remarks>
        /// <param name="workItemId">The ID of the work item to comment on</param>
        /// <param name="request">Comment data</param>
        /// <returns>The created comment</returns>
        /// <response code="201">Returns the newly created comment</response>
        /// <response code="400">If parameters are invalid</response>
        /// <response code="404">If the work item doesn't exist</response>
        /// <response code="409">If commenting is not allowed (e.g., closed work item)</response>
        [HttpPost("{workItemId}/comments")]
        [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CommentResponse>> AddComment(Guid workItemId, AddCommentRequest request)
        {
            var comment = await _workItemService.AddCommentAsync(workItemId, request);
            return CreatedAtAction(nameof(GetById), new { id = workItemId }, comment);
        }
    }
}