using Application.Commons.Interfaces;
using Application.Commons.Interfaces.Data;
using Application.Commons.Models.CustomResult;
using Application.TodoLists.Commands.CreateTodoList;
using Domain.Commons.Enums;
using Domain.TodoLists;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace Application.UnitTests.TodoLists.Commands;

public class CreateTodoListCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateTodoList_AndReturnSuccessResult()
    {
        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoList>>();
        var mockLogService = new Mock<ILogService>();
        var todoLists = new List<TodoList>();

        mockContext.Setup(x => x.TodoLists).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Callback(() => { if (todoLists.Count > 0) todoLists[0].Id = 1; });

        mockDbSet.Setup(x => x.Add(It.IsAny<TodoList>()))
            .Callback<TodoList>(list => todoLists.Add(list))
            .Returns((EntityEntry<TodoList>)null!);

        var handler = new CreateTodoListCommandHandler(mockContext.Object, mockLogService.Object);
        var command = new CreateTodoListCommand
        {
            Title = "Test Todo List"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        result.Result.Should().BeGreaterThan(0);
        todoLists.Should().HaveCount(1);
        todoLists[0].Title.Should().Be("Test Todo List");
        todoLists[0].Colour.Should().Be(Domain.TodoLists.ValueObjects.Colour.White);
        mockContext.Verify(x => x.TodoLists.Add(It.IsAny<TodoList>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockLogService.Verify(x => x.DbLog(It.IsAny<string>(), It.IsAny<Microsoft.Extensions.Logging.LogLevel>()), Times.Once);
    }
}

