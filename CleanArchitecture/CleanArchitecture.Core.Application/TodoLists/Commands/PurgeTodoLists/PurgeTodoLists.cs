using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Command;
using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Domain.Common.Enum;
using CleanArchitecture.Core.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Core.Application.TodoLists.Commands.PurgeTodoLists;

//[Authorize(Roles = Roles.Administrator)]
//[Authorize(Policy = Policies.CanPurge)]
public record PurgeTodoListsCommand : ICommand;

public class PurgeTodoListsCommandHandler(IApplicationDbContext context) : ICommandHandler<PurgeTodoListsCommand>
{
    public async Task<CrudResult> Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
    {
        context.TodoLists.RemoveRange(context.TodoLists);

        await context.SaveChangesAsync(cancellationToken);

        return new CrudResult(CrudStatus.Succeeded);
    }
}
