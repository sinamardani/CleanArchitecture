using Application.Features.TodoItems.EventHandlers;
using Domain.TodoItems;
using Domain.TodoItems.Events;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Unit.Application.TodoItems.EventHandlers;

public class TodoItemCreatedEventHandlerTests
{
    [Fact]
    public async Task Handle_ShouldLogInformation()
    {
        var mockLogger = new Mock<ILogger<TodoItemCreatedEventHandler>>();
        var handler = new TodoItemCreatedEventHandler(mockLogger.Object);

        var todoItem = new TodoItem
        {
            Id = 1,
            ListId = 1,
            Title = "Test Item"
        };

        var domainEvent = new TodoItemCreatedEvent(todoItem);

        await handler.Handle(domainEvent, CancellationToken.None);

        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("TodoItemCreatedEvent")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

