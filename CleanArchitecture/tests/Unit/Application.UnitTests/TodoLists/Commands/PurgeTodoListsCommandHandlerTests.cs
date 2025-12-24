using Application.Common.Interfaces.Data;
using Application.Features.TodoLists.Commands.PurgeTodoLists;
using Domain.Commons.Enums;
using Domain.TodoLists;
using Shared.Models.CustomResult;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Unit.Application.TodoLists.Commands;

public class PurgeTodoListsCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldRemoveAllTodoLists_AndReturnSuccessResult()
    {
        var todoLists = new List<TodoList>
        {
            new() { Id = 1, Title = "List 1" },
            new() { Id = 2, Title = "List 2" },
            new() { Id = 3, Title = "List 3" }
        };

        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoList>>();

        var data = todoLists.AsQueryable();
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.Provider).Returns(data.Provider);
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.Expression).Returns(data.Expression);
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        mockContext.Setup(x => x.TodoLists).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.TodoLists.RemoveRange(It.IsAny<IEnumerable<TodoList>>()));
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(3);

        var handler = new PurgeTodoListsCommandHandler(mockContext.Object);
        var command = new PurgeTodoListsCommand();

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        mockContext.Verify(x => x.TodoLists.RemoveRange(It.IsAny<IEnumerable<TodoList>>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyList_ShouldReturnSuccessResult()
    {
        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoList>>();

        var data = new List<TodoList>().AsQueryable();
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.Provider).Returns(data.Provider);
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.Expression).Returns(data.Expression);
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        mockContext.Setup(x => x.TodoLists).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.TodoLists.RemoveRange(It.IsAny<IEnumerable<TodoList>>()));
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        var handler = new PurgeTodoListsCommandHandler(mockContext.Object);
        var command = new PurgeTodoListsCommand();

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        mockContext.Verify(x => x.TodoLists.RemoveRange(It.IsAny<IEnumerable<TodoList>>()), Times.Once);
    }
}

