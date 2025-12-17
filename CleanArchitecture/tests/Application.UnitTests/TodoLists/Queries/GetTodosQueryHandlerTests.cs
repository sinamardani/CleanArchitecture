using Application.Commons.Interfaces.Data;
using Application.Commons.Models.CustomResult;
using Application.TodoLists.Queries.GetTodos;
using Domain.Commons.Enums;
using Domain.TodoLists;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.TodoLists.Queries;

public class GetTodosQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnTodosVm_WithCorrectData()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new TestDbContext(options);

        var todoLists = new List<TodoList>
        {
            new() { Id = 1, Title = "List 1" },
            new() { Id = 2, Title = "List 2" }
        };

        await context.TodoLists.AddRangeAsync(todoLists);
        await context.SaveChangesAsync();

        var mockContext = new Mock<IApplicationDbContext>();
        mockContext.Setup(x => x.TodoLists).Returns(context.TodoLists);

        var handler = new GetTodosQueryHandler(mockContext.Object);
        var query = new GetTodosQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        result.Result.Should().NotBeNull();
        result.Result.Lists.Should().HaveCount(2);
        result.Result.PriorityLevels.Should().NotBeEmpty();
        result.Result.PriorityLevels.Should().HaveCount(Enum.GetValues<PriorityLevel>().Length);
    }

    private class TestDbContext : DbContext, IApplicationDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TodoList> TodoLists { get; set; } = null!;
        public DbSet<Domain.TodoItems.TodoItem> TodoItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TodoList>(entity =>
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

