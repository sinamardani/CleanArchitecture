using Application.Commons.Interfaces.Data;
using Application.Commons.Interfaces.Messaging.Command;
using Application.Commons.Models.CustomResult;
using Ardalis.GuardClauses;
using Domain.Commons.Enums;

namespace Application.TodoLists.Commands.UpdateTodoList;

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
