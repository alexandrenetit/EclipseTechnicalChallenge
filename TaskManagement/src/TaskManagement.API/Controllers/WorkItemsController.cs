using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/workitems")]
    public class WorkItemsController : ControllerBase
    {
        private readonly IWorkItemService _workItemService;

        public WorkItemsController(IWorkItemService workItemService)
        {
            _workItemService = workItemService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkItemDetailsResponse>> GetById(Guid id)
        {
            var workItem = await _workItemService.GetWorkItemDetailsAsync(id);
            return Ok(workItem);
        }

        [HttpPost]
        public async Task<ActionResult<WorkItemResponse>> Create(CreateWorkItemRequest request)
        {
            var workItem = await _workItemService.CreateWorkItemAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = workItem.Id }, workItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WorkItemResponse>> Update(Guid id, UpdateWorkItemRequest request, [FromHeader] Guid modifiedBy)
        {
            var workItem = await _workItemService.UpdateWorkItemAsync(id, request, modifiedBy);
            return Ok(workItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _workItemService.DeleteWorkItemAsync(id);
            return NoContent();
        }

        [HttpPost("{workItemId}/comments")]
        public async Task<ActionResult<CommentResponse>> AddComment(Guid workItemId, AddCommentRequest request)
        {
            var comment = await _workItemService.AddCommentAsync(workItemId, request);
            return CreatedAtAction(nameof(GetById), new { id = workItemId }, comment);
        }
    }
}