using Application.Common.Interfaces.Data;
using Application.Common.Interfaces.Messaging.Command;
using Ardalis.GuardClauses;
using Domain.Commons.Enums;
using Shared.Models.CustomResult;

namespace Application.Features.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand : ICommand
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }
}

public class UpdateTodoItemCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateTodoItemCommand>
{
    public async Task<CrudResult> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;
        entity.Done = request.Done;

        await context.SaveChangesAsync(cancellationToken);

        return new CrudResult(CrudStatus.Succeeded);
    }
}
