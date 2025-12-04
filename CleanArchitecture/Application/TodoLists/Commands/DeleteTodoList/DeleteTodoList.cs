using Application.Commons.Interfaces.Data;
using Application.Commons.Interfaces.Messaging.Command;
using Application.Commons.Models.CustomResult;
using Ardalis.GuardClauses;
using Domain.Commons.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.TodoLists.Commands.DeleteTodoList;

public record DeleteTodoListCommand(int Id) : ICommand;

public class DeleteTodoListCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteTodoListCommand>
{
    public  async Task<CrudResult> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoLists
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.TodoLists.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        return new CrudResult(CrudStatus.Succeeded);
    }
}
