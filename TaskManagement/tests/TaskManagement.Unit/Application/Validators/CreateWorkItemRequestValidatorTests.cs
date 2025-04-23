using FluentValidation.TestHelper;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Validators;
using TaskManagement.Domain.Enums;
using Xunit;

namespace TaskManagement.Tests.Unit.Application.Validators;

public class CreateWorkItemRequestValidatorTests
{
    private readonly CreateWorkItemRequestValidator _validator;

    public CreateWorkItemRequestValidatorTests()
    {
        _validator = new CreateWorkItemRequestValidator();
    }

    [Fact]
    public void Validator_WithValidRequest_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var request = new CreateWorkItemRequest(
            Title: "Test Work Item",
            Description: "This is a test work item description",
            DueDate: DateTime.Now.AddDays(7),
            Priority: WorkItemPriority.Medium,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    #region Title Validation Tests

    [Fact]
    public void Validator_WithEmptyTitle_ShouldHaveTitleRequiredError()
    {
        // Arrange
        var request = new CreateWorkItemRequest(
            Title: "",
            Description: "Test description",
            DueDate: DateTime.Now.AddDays(7),
            Priority: WorkItemPriority.Medium,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Title)
              .WithErrorMessage("Title is required");
    }

    [Fact]
    public void Validator_WithNullTitle_ShouldHaveTitleRequiredError()
    {
        // Arrange
        var request = new CreateWorkItemRequest(
            Title: null,
            Description: "Test description",
            DueDate: DateTime.Now.AddDays(7),
            Priority: WorkItemPriority.Medium,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Title)
              .WithErrorMessage("Title is required");
    }

    [Fact]
    public void Validator_WithTitleExceeding200Characters_ShouldHaveLengthError()
    {
        // Arrange
        var longTitle = new string('x', 201); // 201 characters
        var request = new CreateWorkItemRequest(
            Title: longTitle,
            Description: "Test description",
            DueDate: DateTime.Now.AddDays(7),
            Priority: WorkItemPriority.Medium,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Title)
              .WithErrorMessage("Title must not exceed 200 characters");
    }

    [Fact]
    public void Validator_WithTitleExactly200Characters_ShouldNotHaveValidationError()
    {
        // Arrange
        var title200Chars = new string('x', 200); // Exactly 200 characters
        var request = new CreateWorkItemRequest(
            Title: title200Chars,
            Description: "Test description",
            DueDate: DateTime.Now.AddDays(7),
            Priority: WorkItemPriority.Medium,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    #endregion Title Validation Tests

    #region Description Validation Tests

    [Fact]
    public void Validator_WithDescriptionExceeding1000Characters_ShouldHaveLengthError()
    {
        // Arrange
        var longDescription = new string('x', 1001); // 1001 characters
        var request = new CreateWorkItemRequest(
            Title: "Test Work Item",
            Description: longDescription,
            DueDate: DateTime.Now.AddDays(7),
            Priority: WorkItemPriority.Medium,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Description)
              .WithErrorMessage("Description must not exceed 1000 characters");
    }

    [Fact]
    public void Validator_WithDescriptionExactly1000Characters_ShouldNotHaveValidationError()
    {
        // Arrange
        var description1000Chars = new string('x', 1000); // Exactly 1000 characters
        var request = new CreateWorkItemRequest(
            Title: "Test Work Item",
            Description: description1000Chars,
            DueDate: DateTime.Now.AddDays(7),
            Priority: WorkItemPriority.Medium,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validator_WithNullDescription_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new CreateWorkItemRequest(
            Title: "Test Work Item",
            Description: null,
            DueDate: DateTime.Now.AddDays(7),
            Priority: WorkItemPriority.Medium,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    #endregion Description Validation Tests

    #region DueDate Validation Tests

    [Fact]
    public void Validator_WithDefaultDueDate_ShouldHaveDueDateRequiredError()
    {
        // Arrange
        var request = new CreateWorkItemRequest(
            Title: "Test Work Item",
            Description: "Test description",
            DueDate: default,
            Priority: WorkItemPriority.Medium,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.DueDate)
              .WithErrorMessage("Due date is required");
    }

    #endregion DueDate Validation Tests

    #region Priority Validation Tests

    [Fact]
    public void Validator_WithInvalidPriority_ShouldHavePriorityError()
    {
        // Arrange - To simulate an invalid enum value, we'll cast an invalid integer
        var invalidPriority = (WorkItemPriority)999;
        var request = new CreateWorkItemRequest(
            Title: "Test Work Item",
            Description: "Test description",
            DueDate: DateTime.Now.AddDays(7),
            Priority: invalidPriority,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Priority)
              .WithErrorMessage("Invalid priority value");
    }

    [Theory]
    [InlineData(WorkItemPriority.Low)]
    [InlineData(WorkItemPriority.Medium)]
    [InlineData(WorkItemPriority.High)]
    public void Validator_WithValidPriority_ShouldNotHaveValidationError(WorkItemPriority priority)
    {
        // Arrange
        var request = new CreateWorkItemRequest(
            Title: "Test Work Item",
            Description: "Test description",
            DueDate: DateTime.Now.AddDays(7),
            Priority: priority,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Priority);
    }

    #endregion Priority Validation Tests

    #region ProjectId Validation Tests

    [Fact]
    public void Validator_WithEmptyProjectId_ShouldHaveProjectIdError()
    {
        // Arrange
        var request = new CreateWorkItemRequest(
            Title: "Test Work Item",
            Description: "Test description",
            DueDate: DateTime.Now.AddDays(7),
            Priority: WorkItemPriority.Medium,
            ProjectId: Guid.Empty,
            CreatedBy: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.ProjectId)
              .WithErrorMessage("Project ID must be a valid GUID");
    }

    #endregion ProjectId Validation Tests

    #region CreatedBy Validation Tests

    [Fact]
    public void Validator_WithEmptyCreatedBy_ShouldHaveCreatedByError()
    {
        // Arrange
        var request = new CreateWorkItemRequest(
            Title: "Test Work Item",
            Description: "Test description",
            DueDate: DateTime.Now.AddDays(7),
            Priority: WorkItemPriority.Medium,
            ProjectId: Guid.NewGuid(),
            CreatedBy: Guid.Empty
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.CreatedBy)
              .WithErrorMessage("Creator ID must be a valid GUID");
    }

    #endregion CreatedBy Validation Tests
}