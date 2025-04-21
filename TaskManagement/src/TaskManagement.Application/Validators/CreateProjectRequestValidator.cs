using FluentValidation;
using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Validators;

public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(100).WithMessage("Project name must not exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Owner ID is required")
            .NotEqual(Guid.Empty).WithMessage("Owner ID must be a valid GUID");
    }
}