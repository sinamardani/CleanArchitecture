using Application.Common.Interfaces.Data;
using Application.Features.TodoLists.Commands.UpdateTodoList;
using Ardalis.GuardClauses;
using Domain.Commons.Enums;
using Domain.TodoLists;
using Shared.Models.CustomResult;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.TodoLists.Commands;

public class UpdateTodoListCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateTodoList_AndReturnSuccessResult()
    {
        var todoList = new TodoList
        {
            Id = 1,
            Title = "Original Title"
        };

        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoList>>();

        mockContext.Setup(x => x.TodoLists).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.TodoLists.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoList);
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new UpdateTodoListCommandHandler(mockContext.Object);
        var command = new UpdateTodoListCommand
        {
            Id = 1,
            Title = "Updated Title"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        todoList.Title.Should().Be("Updated Title");
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoList>>();

        mockContext.Setup(x => x.TodoLists).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.TodoLists.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoList?)null);

        var handler = new UpdateTodoListCommandHandler(mockContext.Object);
        var command = new UpdateTodoListCommand
        {
            Id = 999,
            Title = "Updated Title"
        };

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}

