using Application.Commons.Interfaces.Data;
using Application.Commons.Interfaces.Messaging.Command;
using Application.Commons.Models.CustomResult;
using Domain.Commons.Enums;

namespace Application.TodoLists.Commands.PurgeTodoLists;

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
