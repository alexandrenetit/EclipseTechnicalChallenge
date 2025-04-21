using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repository;
using TaskManagement.Domain.Services;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mappers;

namespace TaskManagement.Application.Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly IWorkItemRepository _workItemRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkItemServiceDomain _domainWorkItemService;

        public WorkItemService(
            IWorkItemRepository workItemRepository,
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork,
            IWorkItemServiceDomain domainWorkItemService)
        {
            _workItemRepository = workItemRepository;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _domainWorkItemService = domainWorkItemService;
        }

        public async Task<WorkItemResponse> CreateWorkItemAsync(CreateWorkItemRequest request)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                throw new KeyNotFoundException("Project not found");
            }

            _domainWorkItemService.ValidateWorkItemCreation(project, request.Priority);

            var workItem = new WorkItem(
                Guid.NewGuid(),
                request.Title,
                request.Description,
                request.DueDate,
                request.Priority,
                request.ProjectId,
                request.CreatedBy);

            project.AddWorkItem(workItem);

            await _workItemRepository.AddAsync(workItem);
            await _unitOfWork.CommitAsync();

            return workItem.ToResponse();
        }

        public async Task<WorkItemDetailsResponse> GetWorkItemDetailsAsync(Guid workItemId)
        {
            var workItem = await _workItemRepository.GetWithDetailsAsync(workItemId);
            if (workItem == null)
            {
                throw new KeyNotFoundException("Work item not found");
            }

            return workItem.ToDetailsResponse();
        }

        public async Task<WorkItemResponse> UpdateWorkItemAsync(Guid workItemId, UpdateWorkItemRequest request, Guid modifiedBy)
        {
            var workItem = await _workItemRepository.GetByIdAsync(workItemId);
            if (workItem == null)
            {
                throw new KeyNotFoundException("Work item not found");
            }

            if (request.Status.HasValue)
            {
                workItem.UpdateStatus(request.Status.Value, modifiedBy);
            }

            if (request.Title != null || request.Description != null || request.DueDate.HasValue)
            {
                workItem.UpdateDetails(
                    request.Title ?? workItem.Title,
                    request.Description ?? workItem.Description,
                    request.DueDate ?? workItem.DueDate,
                    modifiedBy);
            }

            await _unitOfWork.CommitAsync();

            return workItem.ToResponse();
        }

        public async Task DeleteWorkItemAsync(Guid workItemId)
        {
            var workItem = await _workItemRepository.GetByIdAsync(workItemId);
            if (workItem == null)
            {
                throw new KeyNotFoundException("Work item not found");
            }

            if (workItem.Status != WorkItemStatus.Completed)
            {
                throw new InvalidOperationException("Cannot delete pending or in-progress work items");
            }

            await _workItemRepository.DeleteAsync(workItem);
            await _unitOfWork.CommitAsync();
        }

        public async Task<CommentResponse> AddCommentAsync(Guid workItemId, AddCommentRequest request)
        {
            var workItem = await _workItemRepository.GetByIdAsync(workItemId);
            if (workItem == null)
            {
                throw new KeyNotFoundException("Work item not found");
            }

            workItem.AddComment(request.Content, request.AuthorId);
            await _unitOfWork.CommitAsync();

            return workItem.Comments.Last().ToResponse();
        }
    }
}