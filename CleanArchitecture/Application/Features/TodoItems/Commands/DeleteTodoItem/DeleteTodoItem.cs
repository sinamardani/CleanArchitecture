using Application.Common.Interfaces.Data;
using Application.Common.Interfaces.Messaging.Command;
using Ardalis.GuardClauses;
using Domain.Commons.Enums;
using Domain.TodoItems.Events;
using Shared.Models.CustomResult;

namespace Application.Features.TodoItems.Commands.DeleteTodoItem;

public record DeleteTodoItemCommand(int Id) : ICommand;

public class DeleteTodoItemCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteTodoItemCommand>
{
    public async Task<CrudResult> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        context.TodoItems.Remove(entity);

        entity.AddDomainEvent(new TodoItemDeletedEvent(entity));

        await context.SaveChangesAsync(cancellationToken);

        return new CrudResult(CrudStatus.Succeeded);
    }
}
