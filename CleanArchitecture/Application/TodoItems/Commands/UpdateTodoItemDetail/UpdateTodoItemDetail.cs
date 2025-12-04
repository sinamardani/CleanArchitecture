using Application.Commons.Interfaces.Data;
using Application.Commons.Interfaces.Messaging.Command;
using Application.Commons.Models.CustomResult;
using Ardalis.GuardClauses;
using Domain.Commons.Enums;

namespace Application.TodoItems.Commands.UpdateTodoItemDetail;

public record UpdateTodoItemDetailCommand : ICommand
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public PriorityLevel Priority { get; init; }

    public string? Note { get; init; }
}

public class UpdateTodoItemDetailCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateTodoItemDetailCommand>
{
    public async Task<CrudResult> Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.ListId = request.ListId;
        entity.Priority = request.Priority;
        entity.Note = request.Note;

        await context.SaveChangesAsync(cancellationToken);

        return new CrudResult(CrudStatus.Succeeded);
    }
}
