using System;
using FluentValidation.TestHelper;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Validators;
using TaskManagement.Domain.Enums;
using Xunit;

namespace TaskManagement.Tests.Unit.Application.Validators;

public class UpdateWorkItemRequestValidatorTests
{
    private readonly UpdateWorkItemRequestValidator _validator;

    public UpdateWorkItemRequestValidatorTests()
    {
        _validator = new UpdateWorkItemRequestValidator();
    }

    [Fact]
    public void Validator_WithNullValues_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var request = new UpdateWorkItemRequest(
            Title: null,
            Description: null,
            DueDate: null,
            Status: null
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_WithValidValues_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var request = new UpdateWorkItemRequest(
            Title: "Updated Title",
            Description: "Updated Description",
            DueDate: DateTime.Now.AddDays(10),
            Status: WorkItemStatus.InProgress
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    #region Title Validation Tests

    [Fact]
    public void Validator_WithTitleExceeding200Characters_ShouldHaveTitleLengthError()
    {
        // Arrange
        var longTitle = new string('x', 201); // 201 characters
        var request = new UpdateWorkItemRequest(
            Title: longTitle,
            Description: null,
            DueDate: null,
            Status: null
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
        var request = new UpdateWorkItemRequest(
            Title: title200Chars,
            Description: null,
            DueDate: null,
            Status: null
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validator_WithEmptyTitle_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new UpdateWorkItemRequest(
            Title: "",
            Description: null,
            DueDate: null,
            Status: null
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    #endregion

    #region Description Validation Tests

    [Fact]
    public void Validator_WithDescriptionExceeding1000Characters_ShouldHaveDescriptionLengthError()
    {
        // Arrange
        var longDescription = new string('x', 1001); // 1001 characters
        var request = new UpdateWorkItemRequest(
            Title: null,
            Description: longDescription,
            DueDate: null,
            Status: null
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
        var request = new UpdateWorkItemRequest(
            Title: null,
            Description: description1000Chars,
            DueDate: null,
            Status: null
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validator_WithEmptyDescription_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new UpdateWorkItemRequest(
            Title: null,
            Description: "",
            DueDate: null,
            Status: null
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    #endregion

    #region DueDate Validation Tests

    [Fact]
    public void Validator_WithDefaultDueDate_ShouldNotHaveValidationError()
    {
        // Arrange - Using default DateTime without explicitly setting HasValue
        var request = new UpdateWorkItemRequest(
            Title: null,
            Description: null,
            DueDate: default,
            Status: null
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.DueDate);
    }

    [Fact]
    public void Validator_WithValidDueDate_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new UpdateWorkItemRequest(
            Title: null,
            Description: null,
            DueDate: DateTime.Now.AddDays(5),
            Status: null
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.DueDate);
    }

    #endregion

    #region Status Validation Tests

    [Fact]
    public void Validator_WithInvalidStatus_ShouldHaveStatusError()
    {
        // Arrange - To simulate an invalid enum value, we'll cast an invalid integer
        var invalidStatus = (WorkItemStatus)999;
        var request = new UpdateWorkItemRequest(
            Title: null,
            Description: null,
            DueDate: null,
            Status: invalidStatus
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Status)
              .WithErrorMessage("Invalid status value");
    }

    [Theory]
    [InlineData(WorkItemStatus.Pending)]
    [InlineData(WorkItemStatus.InProgress)]
    [InlineData(WorkItemStatus.Completed)]
    public void Validator_WithValidStatus_ShouldNotHaveValidationError(WorkItemStatus status)
    {
        // Arrange
        var request = new UpdateWorkItemRequest(
            Title: null,
            Description: null,
            DueDate: null,
            Status: status
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Status);
    }

    #endregion

    #region Combined Fields Tests

    [Fact]
    public void Validator_WithSomeMixedValidAndInvalidValues_ShouldHaveSpecificErrors()
    {
        // Arrange
        var longTitle = new string('x', 201); // 201 characters
        var invalidStatus = (WorkItemStatus)999;

        var request = new UpdateWorkItemRequest(
            Title: longTitle,
            Description: "Valid description",
            DueDate: DateTime.Now.AddDays(5),
            Status: invalidStatus
        );

        // Act & Assert
        var result = _validator.TestValidate(request);

        // Should have errors for Title and Status
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Status);

        // Should NOT have errors for Description and DueDate
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.DueDate);
    }

    #endregion
}