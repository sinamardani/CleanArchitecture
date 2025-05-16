using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Command;
using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Domain.Common.Enum;

namespace CleanArchitecture.Core.Application.TodoItems.Commands.UpdateTodoItem;

public sealed record UpdateTodoIemCommand : ICommand
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public bool Done { get; init; }
}
public sealed class UpdateTodoItemCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateTodoIemCommand>
{
    public async Task<CrudResult> Handle(UpdateTodoIemCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);


        if (entity == null)
            return new CrudResult(CrudStatus.NotFound, "داده ای یافت نشد");

        entity.Title = request.Title;
        entity.Done = request.Done;

        await context.SaveChangesAsync(cancellationToken);

        return new CrudResult(CrudStatus.Succeeded);
    }
}