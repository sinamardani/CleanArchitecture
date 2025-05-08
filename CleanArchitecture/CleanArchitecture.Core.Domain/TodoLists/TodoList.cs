using CleanArchitecture.Core.Domain.Common;
using CleanArchitecture.Core.Domain.TodoItems;
using CleanArchitecture.Core.Domain.TodoLists.ValueObjects;

namespace CleanArchitecture.Core.Domain.TodoLists;

public sealed class TodoList : BaseAuditTableEntity
{
    public string? Title { get; set; }

    public Colour Colour { get; set; } = Colour.White;

    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
}