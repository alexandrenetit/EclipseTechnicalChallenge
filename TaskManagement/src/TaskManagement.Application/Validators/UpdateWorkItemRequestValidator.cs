using FluentValidation;
using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Validators;

public class UpdateWorkItemRequestValidator : AbstractValidator<UpdateWorkItemRequest>
{
    public UpdateWorkItemRequestValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters")
            .When(x => x.Title != null);

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
            .When(x => x.Description != null);

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("Due date is required")
            .When(x => x.DueDate.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status value")
            .When(x => x.Status.HasValue);
    }
}