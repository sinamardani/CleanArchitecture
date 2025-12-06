using Domain.TodoLists;
using Mapster;

namespace Application.TodoLists.Queries.GetTodos;

public class TodoListDto : IRegister
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public string? Colour { get; init; }

    public IReadOnlyCollection<TodoItemDto> Items { get; init; } = Array.Empty<TodoItemDto>();

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TodoList, TodoListDto>();
    }
}
