using Application.Common.Interfaces.Data;
using Application.Common.Interfaces.Messaging.Command;
using Ardalis.GuardClauses;
using Domain.Commons.Enums;
using Shared.Models.CustomResult;

namespace Application.Features.TodoLists.Commands.UpdateTodoList;

public record UpdateTodoListCommand : ICommand
{
    public int Id { get; init; }

    public string? Title { get; init; }
}

public class UpdateTodoListCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateTodoListCommand>
{
    public async Task<CrudResult> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoLists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;

        await context.SaveChangesAsync(cancellationToken);

        return new CrudResult(CrudStatus.Succeeded);
    }
}
