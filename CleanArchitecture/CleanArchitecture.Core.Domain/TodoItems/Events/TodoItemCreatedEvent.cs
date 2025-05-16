using CleanArchitecture.Core.Domain.Common;

namespace CleanArchitecture.Core.Domain.TodoItems.Events;

public sealed record TodoItemCreatedEvent(TodoItem Item) : BaseEvent;