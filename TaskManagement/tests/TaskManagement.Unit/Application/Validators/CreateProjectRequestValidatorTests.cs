using FluentValidation.TestHelper;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Validators;
using Xunit;

namespace TaskManagement.Tests.Unit.Application.Validators;

public class CreateProjectRequestValidatorTests
{
    private readonly CreateProjectRequestValidator _validator;

    public CreateProjectRequestValidatorTests()
    {
        _validator = new CreateProjectRequestValidator();
    }

    [Fact]
    public void Validator_WithValidRequest_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var request = new CreateProjectRequest(
            Name: "Test Project",
            Description: "This is a test project description",
            OwnerId: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_WithEmptyName_ShouldHaveNameRequiredError()
    {
        // Arrange
        var request = new CreateProjectRequest(
            Name: "",
            Description: "This is a test project description",
            OwnerId: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Project name is required");
    }

    [Fact]
    public void Validator_WithNullName_ShouldHaveNameRequiredError()
    {
        // Arrange
        var request = new CreateProjectRequest(
            Name: null,
            Description: "This is a test project description",
            OwnerId: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Project name is required");
    }

    [Fact]
    public void Validator_WithNameExceeding100Characters_ShouldHaveLengthError()
    {
        // Arrange
        var longName = new string('x', 101); // 101 characters
        var request = new CreateProjectRequest(
            Name: longName,
            Description: "This is a test project description",
            OwnerId: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Project name must not exceed 100 characters");
    }

    [Fact]
    public void Validator_WithDescriptionExceeding500Characters_ShouldHaveLengthError()
    {
        // Arrange
        var longDescription = new string('x', 501); // 501 characters
        var request = new CreateProjectRequest(
            Name: "Test Project",
            Description: longDescription,
            OwnerId: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Description)
              .WithErrorMessage("Description must not exceed 500 characters");
    }

    [Fact]
    public void Validator_WithEmptyOwnerId_ShouldHaveOwnerIdRequiredError()
    {
        // Arrange
        var request = new CreateProjectRequest(
            Name: "Test Project",
            Description: "This is a test project description",
            OwnerId: Guid.Empty
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.OwnerId)
              .WithErrorMessage("Owner ID must be a valid GUID");
    }

    [Fact]
    public void Validator_WithValidNameAt100Characters_ShouldNotHaveNameLengthError()
    {
        // Arrange
        var name100Chars = new string('x', 100); // Exactly 100 characters
        var request = new CreateProjectRequest(
            Name: name100Chars,
            Description: "This is a test project description",
            OwnerId: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validator_WithValidDescriptionAt500Characters_ShouldNotHaveDescriptionLengthError()
    {
        // Arrange
        var description500Chars = new string('x', 500); // Exactly 500 characters
        var request = new CreateProjectRequest(
            Name: "Test Project",
            Description: description500Chars,
            OwnerId: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validator_WithNullDescription_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new CreateProjectRequest(
            Name: "Test Project",
            Description: null,
            OwnerId: Guid.NewGuid()
        );

        // Act & Assert
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
}