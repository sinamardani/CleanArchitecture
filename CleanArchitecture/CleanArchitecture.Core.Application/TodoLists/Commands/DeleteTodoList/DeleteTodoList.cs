using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Command;
using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Domain.Common.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Core.Application.TodoLists.Commands.DeleteTodoList;

public record DeleteTodoListCommand(int Id) : ICommand;

public class DeleteTodoListCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteTodoListCommand>
{
    public async Task<CrudResult> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoLists
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);
        if (entity == null)
            return new CrudResult(CrudStatus.NotFound, "داده ای یافت نشد");
        context.TodoLists.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
        return new CrudResult(CrudStatus.Succeeded);
    }
}
