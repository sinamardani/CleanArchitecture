using Application.Common.Interfaces.Data;
using Application.Features.TodoItems.Commands.DeleteTodoItem;
using Ardalis.GuardClauses;
using Domain.Commons.Enums;
using Domain.TodoItems;
using Shared.Models.CustomResult;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Unit.Application.TodoItems.Commands;

public class DeleteTodoItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteTodoItem_AndReturnSuccessResult()
    {
        var todoItem = new TodoItem
        {
            Id = 1,
            ListId = 1,
            Title = "Test Item",
            Done = false
        };

        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoItem>>();

        mockContext.Setup(x => x.TodoItems).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.TodoItems.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoItem);
        mockContext.Setup(x => x.TodoItems.Remove(It.IsAny<TodoItem>()));
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new DeleteTodoItemCommandHandler(mockContext.Object);
        var command = new DeleteTodoItemCommand(1);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        mockContext.Verify(x => x.TodoItems.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Once);
        mockContext.Verify(x => x.TodoItems.Remove(It.IsAny<TodoItem>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldAddDomainEvent_WhenDeletingTodoItem()
    {
        var todoItem = new TodoItem
        {
            Id = 1,
            ListId = 1,
            Title = "Test Item",
            Done = false
        };

        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoItem>>();

        mockContext.Setup(x => x.TodoItems).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.TodoItems.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoItem);
        mockContext.Setup(x => x.TodoItems.Remove(It.IsAny<TodoItem>()));
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new DeleteTodoItemCommandHandler(mockContext.Object);
        var command = new DeleteTodoItemCommand(1);

        await handler.Handle(command, CancellationToken.None);

        todoItem.DomainEvents.Should().ContainSingle();
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoItem>>();

        mockContext.Setup(x => x.TodoItems).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.TodoItems.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoItem?)null);

        var handler = new DeleteTodoItemCommandHandler(mockContext.Object);
        var command = new DeleteTodoItemCommand(999);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}

