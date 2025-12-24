using Domain.TodoItems;
using Domain.TodoItems.Events;
using FluentAssertions;

namespace Unit.Domain.TodoItems;

public class TodoItemTests
{
    [Fact]
    public void SetDone_ToTrue_WhenFalse_ShouldRaiseTodoItemCompletedEvent()
    {
        var todoItem = new TodoItem
        {
            ListId = 1,
            Title = "Test Item",
            Done = false
        };

        todoItem.ClearDomainEvents();
        todoItem.Done = true;

        todoItem.Done.Should().BeTrue();
        todoItem.DomainEvents.Should().ContainSingle();
        todoItem.DomainEvents.Should().Contain(e => e is TodoItemCompletedEvent);
    }

    [Fact]
    public void SetDone_ToTrue_WhenAlreadyTrue_ShouldNotRaiseEvent()
    {
        var todoItem = new TodoItem
        {
            ListId = 1,
            Title = "Test Item",
            Done = true
        };

        todoItem.ClearDomainEvents();
        todoItem.Done = true;

        todoItem.Done.Should().BeTrue();
        todoItem.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void SetDone_ToFalse_ShouldNotRaiseEvent()
    {
        var todoItem = new TodoItem
        {
            ListId = 1,
            Title = "Test Item",
            Done = true
        };

        todoItem.ClearDomainEvents();
        todoItem.Done = false;

        todoItem.Done.Should().BeFalse();
        todoItem.DomainEvents.Should().BeEmpty();
    }
}

