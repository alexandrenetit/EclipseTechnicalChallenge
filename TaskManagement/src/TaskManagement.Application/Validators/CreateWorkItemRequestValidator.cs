using FluentValidation;
using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Validators;

public class CreateWorkItemRequestValidator : AbstractValidator<CreateWorkItemRequest>
{
    public CreateWorkItemRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("Due date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Invalid priority value");

        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("Project ID is required")
            .NotEqual(Guid.Empty).WithMessage("Project ID must be a valid GUID");

        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("Creator ID is required")
            .NotEqual(Guid.Empty).WithMessage("Creator ID must be a valid GUID");
    }
}