using Application.Features.TodoLists.Commands.CreateTodoList;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Integration.Application.TodoLists.Validators;

public class CreateTodoListCommandValidatorTests
{
    [Fact]
    public async Task Validate_WithValidCommand_ShouldPass()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var validator = new CreateTodoListCommandValidator(context);
        var command = new CreateTodoListCommand
        {
            Title = "Unique Title"
        };

        var result = await validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithEmptyTitle_ShouldFail()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var validator = new CreateTodoListCommandValidator(context);
        var command = new CreateTodoListCommand
        {
            Title = string.Empty
        };

        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public async Task Validate_WithDuplicateTitle_ShouldFail()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var existingList = new Domain.TodoLists.TodoList
        {
            Title = "Existing Title"
        };

        await context.TodoLists.AddAsync(existingList);
        await context.SaveChangesAsync();

        var validator = new CreateTodoListCommandValidator(context);
        var command = new CreateTodoListCommand
        {
            Title = "Existing Title"
        };

        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public async Task Validate_WithTitleExceedingMaxLength_ShouldFail()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var validator = new CreateTodoListCommandValidator(context);
        var command = new CreateTodoListCommand
        {
            Title = new string('A', 201)
        };

        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
}

