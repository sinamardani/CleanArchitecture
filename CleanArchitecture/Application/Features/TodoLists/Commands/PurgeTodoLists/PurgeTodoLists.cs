using Application.Common.Interfaces.Data;
using Application.Common.Interfaces.Messaging.Command;
using Domain.Commons.Enums;
using Shared.Models.CustomResult;

namespace Application.Features.TodoLists.Commands.PurgeTodoLists;

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
