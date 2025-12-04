using Domain.TodoLists;
using Mapster;

namespace Application.Commons.Models;

public class LookupDto : IRegister
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TodoList, LookupDto>();
        config.NewConfig<TodoList, LookupDto>();
    }
}
