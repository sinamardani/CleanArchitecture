using Application.Common.Interfaces.Data;
using Application.Features.TodoItems.Commands.UpdateTodoItemDetail;
using Ardalis.GuardClauses;
using Domain.Commons.Enums;
using Domain.TodoItems;
using Shared.Models.CustomResult;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Unit.Application.TodoItems.Commands;

public class UpdateTodoItemDetailCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateTodoItemDetail_AndReturnSuccessResult()
    {
        var todoItem = new TodoItem
        {
            Id = 1,
            ListId = 1,
            Title = "Original Title",
            Priority = PriorityLevel.None,
            Note = "Original Note"
        };

        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoItem>>();

        mockContext.Setup(x => x.TodoItems).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.TodoItems.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoItem);
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new UpdateTodoItemDetailCommandHandler(mockContext.Object);
        var command = new UpdateTodoItemDetailCommand
        {
            Id = 1,
            ListId = 2,
            Priority = PriorityLevel.High,
            Note = "Updated Note"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        todoItem.ListId.Should().Be(2);
        todoItem.Priority.Should().Be(PriorityLevel.High);
        todoItem.Note.Should().Be("Updated Note");
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

        var handler = new UpdateTodoItemDetailCommandHandler(mockContext.Object);
        var command = new UpdateTodoItemDetailCommand
        {
            Id = 999,
            ListId = 1,
            Priority = PriorityLevel.Medium,
            Note = "Test Note"
        };

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}

