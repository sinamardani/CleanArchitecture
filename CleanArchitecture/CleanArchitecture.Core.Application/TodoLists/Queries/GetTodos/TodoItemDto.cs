using CleanArchitecture.Core.Domain.TodoItems;
using Mapster;

namespace CleanArchitecture.Core.Application.TodoLists.Queries.GetTodos;

public class TodoItemDto
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }

    public int Priority { get; init; }

    public string? Note { get; init; }

    private class Mapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TodoItem, TodoItemDto>()
                .Map(src=>src.Priority , dest => (int)dest.Priority);
        }
    }
}
