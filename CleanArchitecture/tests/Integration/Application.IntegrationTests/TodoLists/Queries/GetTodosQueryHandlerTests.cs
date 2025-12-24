using Application.Features.TodoLists.Queries.GetTodos;
using Domain.Commons.Enums;
using Domain.TodoLists;
using Shared.Models.CustomResult;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Integration.Application.TodoLists.Queries;

public class GetTodosQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnTodosVm_WithCorrectData()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var todoLists = new List<TodoList>
        {
            new() { Id = 1, Title = "List 1" },
            new() { Id = 2, Title = "List 2" },
            new() { Id = 3, Title = "List 3" }
        };

        await context.TodoLists.AddRangeAsync(todoLists);
        await context.SaveChangesAsync();

        var handler = new GetTodosQueryHandler(context);
        var query = new GetTodosQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        result.Result.Should().NotBeNull();
        result.Result.Lists.Should().HaveCount(3);
        result.Result.PriorityLevels.Should().NotBeEmpty();
        result.Result.PriorityLevels.Should().HaveCount(Enum.GetValues<PriorityLevel>().Length);
    }

    [Fact]
    public async Task Handle_ShouldReturnPriorityLevels_WithAllEnumValues()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var handler = new GetTodosQueryHandler(context);
        var query = new GetTodosQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        result.Result.PriorityLevels.Should().HaveCount(Enum.GetValues<PriorityLevel>().Length);
        
        var priorityLevels = Enum.GetValues<PriorityLevel>();
        foreach (var priority in priorityLevels)
        {
            result.Result.PriorityLevels.Should().Contain(p => p.Title == priority.ToString());
        }
    }

    [Fact]
    public async Task Handle_ShouldOrderTodoListsByTitle()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var todoLists = new List<TodoList>
        {
            new() { Title = "Zebra List" },
            new() { Title = "Apple List" },
            new() { Title = "Banana List" }
        };

        await context.TodoLists.AddRangeAsync(todoLists);
        await context.SaveChangesAsync();

        var handler = new GetTodosQueryHandler(context);
        var query = new GetTodosQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        result.Result.Lists.Should().HaveCount(3);
        result.Result.Lists.ElementAt(0).Title.Should().Be("Apple List");
        result.Result.Lists.ElementAt(1).Title.Should().Be("Banana List");
        result.Result.Lists.ElementAt(2).Title.Should().Be("Zebra List");
    }

    [Fact]
    public async Task Handle_WithEmptyDatabase_ShouldReturnEmptyLists()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var handler = new GetTodosQueryHandler(context);
        var query = new GetTodosQuery();

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        result.Result.Lists.Should().BeEmpty();
        result.Result.PriorityLevels.Should().NotBeEmpty();
    }
}
