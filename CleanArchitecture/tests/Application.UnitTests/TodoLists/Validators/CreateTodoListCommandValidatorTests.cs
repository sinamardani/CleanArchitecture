using Application.Commons.Interfaces.Data;
using Application.TodoLists.Commands.CreateTodoList;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.TodoLists.Validators;

public class CreateTodoListCommandValidatorTests
{
    [Fact]
    public async Task Validate_WithValidCommand_ShouldPass()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new TestDbContext(options);

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
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new TestDbContext(options);

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
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new TestDbContext(options);

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
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new TestDbContext(options);

        var validator = new CreateTodoListCommandValidator(context);
        var command = new CreateTodoListCommand
        {
            Title = new string('A', 201)
        };

        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    private class TestDbContext : DbContext, IApplicationDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<Domain.TodoLists.TodoList> TodoLists { get; set; } = null!;
        public DbSet<Domain.TodoItems.TodoItem> TodoItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Domain.TodoLists.TodoList>(entity =>
            {
                entity.OwnsOne(e => e.Colour);
            });
        }

        Task<int> IApplicationDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

