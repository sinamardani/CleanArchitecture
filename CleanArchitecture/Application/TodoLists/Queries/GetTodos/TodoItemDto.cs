using Domain.TodoItems;
using Mapster;

namespace Application.TodoLists.Queries.GetTodos;

public class TodoItemDto : IRegister
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }

    public int Priority { get; init; }

    public string? Note { get; init; }


    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TodoItem, TodoItemDto>()
            .Map(dest => dest.Priority, src => (int)src.Priority);
    }
}
