using Application.Features.TodoItems.Commands.UpdateTodoItem;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Unit.Application.TodoItems.Validators;

public class UpdateTodoItemCommandValidatorTests
{
    private readonly UpdateTodoItemCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        var command = new UpdateTodoItemCommand
        {
            Id = 1,
            Title = "Valid Title",
            Done = false
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyTitle_ShouldFail()
    {
        var command = new UpdateTodoItemCommand
        {
            Id = 1,
            Title = string.Empty,
            Done = false
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithNullTitle_ShouldFail()
    {
        var command = new UpdateTodoItemCommand
        {
            Id = 1,
            Title = null,
            Done = false
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithTitleExceedingMaxLength_ShouldFail()
    {
        var command = new UpdateTodoItemCommand
        {
            Id = 1,
            Title = new string('A', 201),
            Done = false
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
}

