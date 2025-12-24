using Application.Common.Interfaces.Data;
using Application.Common.Interfaces.Messaging.Command;
using Application.Common.Mappings;
using Domain.Commons.Enums;
using Domain.TodoLists;
using Shared.Models.CustomResult;

namespace Application.Features.TodoLists.Queries.GetTodos;

public record GetTodosQuery : ICommand<TodosVm>;

public class GetTodosQueryHandler(IApplicationDbContext context)
    : ICommandHandler<GetTodosQuery, TodosVm>
{
    public async Task<CrudResult<TodosVm>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        var priorityLevels = Enum.GetValues<PriorityLevel>()
            .Select(p => new LookupDto { Id = (int)p, Title = p.ToString() })
            .ToArray();

        var lists = await context.TodoLists
            .OrderBy(t => t.Title)
            .ProjectToListAsync<TodoList, TodoListDto>(cancellationToken);

        var result = new TodosVm()
        {
            Lists = lists,
            PriorityLevels = priorityLevels
        };

        return new CrudResult<TodosVm>(CrudStatus.Succeeded, result);
    }
}
