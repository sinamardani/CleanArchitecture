using Domain.TodoItems;
using Domain.TodoLists;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.UnitTests.Data;

public class ApplicationDbContextTests
{
    [Fact]
    public void ApplicationDbContext_ShouldHaveTodoItemsDbSet()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        context.TodoItems.Should().NotBeNull();
    }

    [Fact]
    public void ApplicationDbContext_ShouldHaveTodoListsDbSet()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        context.TodoLists.Should().NotBeNull();
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldSaveTodoItem()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var todoItem = new TodoItem
        {
            ListId = 1,
            Title = "Test Item",
            Done = false
        };

        context.TodoItems.Add(todoItem);
        var result = await context.SaveChangesAsync();

        result.Should().BeGreaterThan(0);
        context.TodoItems.Should().Contain(todoItem);
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldSaveTodoList()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var todoList = new TodoList
        {
            Title = "Test List"
        };

        context.TodoLists.Add(todoList);
        var result = await context.SaveChangesAsync();

        result.Should().BeGreaterThan(0);
        context.TodoLists.Should().Contain(todoList);
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldQueryTodoItems()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var todoItems = new List<TodoItem>
        {
            new() { ListId = 1, Title = "Item 1", Done = false },
            new() { ListId = 1, Title = "Item 2", Done = true }
        };

        await context.TodoItems.AddRangeAsync(todoItems);
        await context.SaveChangesAsync();

        var result = await context.TodoItems.Where(x => x.ListId == 1).ToListAsync();

        result.Should().HaveCount(2);
    }
}
