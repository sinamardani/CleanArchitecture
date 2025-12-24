using Domain.TodoItems;
using Mapster;

namespace Application.Features.TodoItems.Queries.GetTodoItemsWithPagination;

public class TodoItemBriefDto : IRegister
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TodoItem, TodoItemBriefDto>();
    }
}
