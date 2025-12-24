using Application.Features.TodoItems.Commands.CreateTodoItem;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Unit.Application.TodoItems.Validators;

public class CreateTodoItemCommandValidatorTests
{
    private readonly CreateTodoItemCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        var command = new CreateTodoItemCommand
        {
            ListId = 1,
            Title = "Valid Title"
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyTitle_ShouldFail()
    {
        var command = new CreateTodoItemCommand
        {
            ListId = 1,
            Title = string.Empty
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithNullTitle_ShouldFail()
    {
        var command = new CreateTodoItemCommand
        {
            ListId = 1,
            Title = null
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithTitleExceedingMaxLength_ShouldFail()
    {
        var command = new CreateTodoItemCommand
        {
            ListId = 1,
            Title = new string('A', 201)
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithTitleAtMaxLength_ShouldPass()
    {
        var command = new CreateTodoItemCommand
        {
            ListId = 1,
            Title = new string('A', 200)
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}

