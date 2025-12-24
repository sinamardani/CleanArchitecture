using Application.Common.Interfaces.Data;
using Application.Features.TodoItems.Commands.UpdateTodoItem;
using Ardalis.GuardClauses;
using Domain.Commons.Enums;
using Domain.TodoItems;
using Shared.Models.CustomResult;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.TodoItems.Commands;

public class UpdateTodoItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateTodoItem_AndReturnSuccessResult()
    {
        var todoItem = new TodoItem
        {
            Id = 1,
            ListId = 1,
            Title = "Original Title",
            Done = false
        };

        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoItem>>();

        mockContext.Setup(x => x.TodoItems).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.TodoItems.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoItem);
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new UpdateTodoItemCommandHandler(mockContext.Object);
        var command = new UpdateTodoItemCommand
        {
            Id = 1,
            Title = "Updated Title",
            Done = true
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        todoItem.Title.Should().Be("Updated Title");
        todoItem.Done.Should().BeTrue();
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoItem>>();

        mockContext.Setup(x => x.TodoItems).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.TodoItems.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoItem?)null);

        var handler = new UpdateTodoItemCommandHandler(mockContext.Object);
        var command = new UpdateTodoItemCommand
        {
            Id = 999,
            Title = "Updated Title",
            Done = true
        };

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}

