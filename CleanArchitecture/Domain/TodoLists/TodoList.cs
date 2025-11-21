using Domain.Commons;
using Domain.TodoItems;
using Domain.TodoLists.ValueObjects;

namespace Domain.TodoLists;

public sealed class TodoList : BaseAuditTableEntity
{
    public string? Title { get; set; }

    public Colour Colour { get; set; } = Colour.White;

    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
}