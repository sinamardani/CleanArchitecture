using CleanArchitecture.Core.Domain.TodoItems;
using CleanArchitecture.Core.Domain.TodoLists;
using Mapster;

namespace CleanArchitecture.Core.Application.Common.Models;

public class LookupDto
{
    public int Id { get; init; }

    public string? Title { get; init; }

    private class Mapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TodoList, LookupDto>();
            config.NewConfig<TodoItem, LookupDto>();
        }
    }
}
