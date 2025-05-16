using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Command;
using CleanArchitecture.Core.Application.Common.Models;
using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Domain.Common.Enum;
using CleanArchitecture.Core.Domain.TodoItems.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Core.Application.TodoLists.Queries.GetTodos;

//[Authorize]
public sealed record GetTodosQuery : ICommand<TodosVm>;

public sealed class GetTodosQueryHandler(IApplicationDbContext context) : ICommandHandler<GetTodosQuery, TodosVm>
{
    public async Task<CrudResult<TodosVm>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        var todo = new TodosVm
        {
            PriorityLevels = Enum.GetValues(typeof(PriorityLevel))
                .Cast<PriorityLevel>()
                .Select(p => new LookupDto { Id = (int)p, Title = p.ToString() })
                .ToList(),

            Lists = await context.TodoLists
                .AsNoTracking()
                .ProjectToType<TodoListDto>()
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken)
        };

        return new CrudResult<TodosVm>(CrudStatus.Succeeded, todo);
    }
}
