using CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Command;
using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Domain.TodoItems.Events;
using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using CleanArchitecture.Core.Domain.Common.Enum;

namespace CleanArchitecture.Core.Application.TodoItems.Commands.DeleteTodoItem;


public sealed record DeleteTodoItemCommand(int Id) : ICommand;

public sealed class DeleteTodoItemCommandHandler (IApplicationDbContext context): ICommandHandler<DeleteTodoItemCommand>
{
    public async Task<CrudResult> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            return new CrudResult(CrudStatus.NotFound, "داده ای یافت نشد");

        context.TodoItems.Remove(entity);

        entity.AddDomainEvent(new TodoItemDeletedEvent(entity));

        await context.SaveChangesAsync(cancellationToken);

        return new CrudResult(CrudStatus.Succeeded);
    }
}