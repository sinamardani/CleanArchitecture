using Domain.Commons;

namespace Domain.TodoItems.Events;

public sealed record TodoItemCreatedEvent(TodoItem Item) : BaseEvent;