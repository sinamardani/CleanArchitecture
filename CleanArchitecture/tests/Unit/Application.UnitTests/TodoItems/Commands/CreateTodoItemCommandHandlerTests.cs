using Application.Common.Interfaces.Data;
using Application.Features.TodoItems.Commands.CreateTodoItem;
using Domain.Commons.Enums;
using Domain.TodoItems;
using Shared.Models.CustomResult;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace Unit.Application.TodoItems.Commands;

public class CreateTodoItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateTodoItem_AndReturnSuccessResult()
    {
        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoItem>>();
        var todoItems = new List<TodoItem>();

        mockContext.Setup(x => x.TodoItems).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Callback(() => { if (todoItems.Count > 0) todoItems[0].Id = 1; });

        mockDbSet.Setup(x => x.Add(It.IsAny<TodoItem>()))
            .Callback<TodoItem>(item => todoItems.Add(item))
            .Returns((EntityEntry<TodoItem>)null!);

        var handler = new CreateTodoItemCommandHandler(mockContext.Object);
        var command = new CreateTodoItemCommand
        {
            ListId = 1,
            Title = "Test Todo Item"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        result.Result.Should().BeGreaterThan(0);
        todoItems.Should().HaveCount(1);
        todoItems[0].Title.Should().Be("Test Todo Item");
        todoItems[0].ListId.Should().Be(1);
        todoItems[0].Done.Should().BeFalse();
        mockContext.Verify(x => x.TodoItems.Add(It.IsAny<TodoItem>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldAddDomainEvent_WhenCreatingTodoItem()
    {
        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoItem>>();
        var todoItems = new List<TodoItem>();

        mockContext.Setup(x => x.TodoItems).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        mockDbSet.Setup(x => x.Add(It.IsAny<TodoItem>()))
            .Callback<TodoItem>(item => todoItems.Add(item))
            .Returns((EntityEntry<TodoItem>)null!);

        var handler = new CreateTodoItemCommandHandler(mockContext.Object);
        var command = new CreateTodoItemCommand
        {
            ListId = 1,
            Title = "Test Todo Item"
        };

        await handler.Handle(command, CancellationToken.None);

        todoItems.Should().HaveCount(1);
        todoItems[0].DomainEvents.Should().ContainSingle();
    }
}

