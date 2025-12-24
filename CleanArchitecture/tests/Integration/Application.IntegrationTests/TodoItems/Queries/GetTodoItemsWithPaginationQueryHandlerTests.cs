using Application.Features.TodoItems.Queries.GetTodoItemsWithPagination;
using Domain.Commons.Enums;
using Domain.TodoItems;
using Shared.Models.CustomResult;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Integration.Application.TodoItems.Queries;

public class GetTodoItemsWithPaginationQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WithCorrectData()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        
        var todoItems = new List<TodoItem>
        {
            new() { Id = 1, ListId = 1, Title = "Item 1", Done = false },
            new() { Id = 2, ListId = 1, Title = "Item 2", Done = true },
            new() { Id = 3, ListId = 2, Title = "Item 3", Done = false },
            new() { Id = 4, ListId = 1, Title = "Item 4", Done = false }
        };

        await context.TodoItems.AddRangeAsync(todoItems);
        await context.SaveChangesAsync();

        var handler = new GetTodoItemsWithPaginationQueryHandler(context);
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
        result.Result.Items.Should().HaveCount(3);
        result.Result.TotalCount.Should().Be(3);
        result.Result.PageNumber.Should().Be(1);
        result.Result.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task Handle_WithPagination_ShouldReturnCorrectPage()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        
        var todoItems = new List<TodoItem>();
        for (int i = 1; i <= 25; i++)
        {
            todoItems.Add(new TodoItem
            {
                ListId = 1,
                Title = $"Item {i}",
                Done = false
            });
        }

        await context.TodoItems.AddRangeAsync(todoItems);
        await context.SaveChangesAsync();

        var handler = new GetTodoItemsWithPaginationQueryHandler(context);
        var query = new GetTodoItemsWithPaginationQuery
        {
            ListId = 1,
            PageNumber = 2,
            PageSize = 10
        };

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        result.Result.Should().NotBeNull();
        result.Result.Items.Should().HaveCount(10);
        result.Result.TotalCount.Should().Be(25);
        result.Result.PageNumber.Should().Be(2);
        result.Result.TotalPages.Should().Be(3);
    }

    [Fact]
    public async Task Handle_WithEmptyList_ShouldReturnEmptyPaginatedList()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var handler = new GetTodoItemsWithPaginationQueryHandler(context);
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
        result.Result.Items.Should().BeEmpty();
        result.Result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldOrderByTitle()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        
        var todoItems = new List<TodoItem>
        {
            new() { ListId = 1, Title = "Zebra", Done = false },
            new() { ListId = 1, Title = "Apple", Done = false },
            new() { ListId = 1, Title = "Banana", Done = false }
        };

        await context.TodoItems.AddRangeAsync(todoItems);
        await context.SaveChangesAsync();

        var handler = new GetTodoItemsWithPaginationQueryHandler(context);
        var query = new GetTodoItemsWithPaginationQuery
        {
            ListId = 1,
            PageNumber = 1,
            PageSize = 10
        };

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        result.Result.Items.Should().HaveCount(3);
        result.Result.Items[0].Title.Should().Be("Apple");
        result.Result.Items[1].Title.Should().Be("Banana");
        result.Result.Items[2].Title.Should().Be("Zebra");
    }
}
