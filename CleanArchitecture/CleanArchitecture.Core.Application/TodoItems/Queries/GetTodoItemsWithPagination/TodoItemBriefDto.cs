using CleanArchitecture.Core.Domain.TodoItems;
using Mapster;

namespace CleanArchitecture.Core.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public class TodoItemBriefDto
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }
    private class Mapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TodoItem, TodoItemBriefDto>();
        }
    }
}
