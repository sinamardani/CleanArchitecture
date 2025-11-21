using Domain.Commons;

namespace Domain.TodoItems.Events;

public sealed record TodoItemDeletedEvent(TodoItem Item) : BaseEvent;