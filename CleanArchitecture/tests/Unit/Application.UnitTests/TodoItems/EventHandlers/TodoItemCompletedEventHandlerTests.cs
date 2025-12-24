using Application.Features.TodoItems.EventHandlers;
using Domain.TodoItems;
using Domain.TodoItems.Events;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Unit.Application.TodoItems.EventHandlers;

public class TodoItemCompletedEventHandlerTests
{
    [Fact]
    public async Task Handle_ShouldLogInformation()
    {
        var mockLogger = new Mock<ILogger<TodoItemCompletedEventHandler>>();
        var handler = new TodoItemCompletedEventHandler(mockLogger.Object);

        var todoItem = new TodoItem
        {
            Id = 1,
            ListId = 1,
            Title = "Test Item",
            Done = true
        };

        var domainEvent = new TodoItemCompletedEvent(todoItem);

        await handler.Handle(domainEvent, CancellationToken.None);

        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("TodoItemCompletedEvent")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

