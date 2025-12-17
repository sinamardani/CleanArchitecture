using Application.Commons.Interfaces.Data;
using Application.Commons.Models.CustomResult;
using Application.TodoItems.Queries.GetTodoItemsWithPagination;
using Domain.Commons.Enums;
using Domain.TodoItems;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.TodoItems.Queries;

public class GetTodoItemsWithPaginationQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WithCorrectData()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new TestDbContext(options);
        
        var todoItems = new List<TodoItem>
        {
            new() { Id = 1, ListId = 1, Title = "Item 1", Done = false },
            new() { Id = 2, ListId = 1, Title = "Item 2", Done = true },
            new() { Id = 3, ListId = 2, Title = "Item 3", Done = false }
        };

        await context.TodoItems.AddRangeAsync(todoItems);
        await context.SaveChangesAsync();

        var mockContext = new Mock<IApplicationDbContext>();
        mockContext.Setup(x => x.TodoItems).Returns(context.TodoItems);

        var handler = new GetTodoItemsWithPaginationQueryHandler(mockContext.Object);
        var query = new GetTodoItemsWithPaginationQuery
        {
            ListId = 1,
            PageNumber = 1,
            PageSize = 10
        };

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        result.Result.Should().NotBeNull();
        result.Result.Items.Should().HaveCount(2);
        result.Result.TotalCount.Should().Be(2);
    }

    private class TestDbContext : DbContext, IApplicationDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DbSet<Domain.TodoLists.TodoList> TodoLists { get; set; } = null!;

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

