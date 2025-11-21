using Domain.Commons;

namespace Domain.TodoItems.Events;

public sealed record TodoItemCompletedEvent(TodoItem Item) : BaseEvent;