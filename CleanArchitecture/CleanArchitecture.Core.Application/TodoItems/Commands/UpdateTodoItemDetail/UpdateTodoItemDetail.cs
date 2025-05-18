using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Command;
using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Domain.Common.Enum;
using CleanArchitecture.Core.Domain.TodoItems.Enums;

namespace CleanArchitecture.Core.Application.TodoItems.Commands.UpdateTodoItemDetail;

public sealed record UpdateTodoItemDetailCommand : ICommand
{
    public int Id { get; init; }
    public int ListId { get; init; }
    public PriorityLevel Priority { get; init; }
    public string? Note { get; init; }
}
public sealed class UpdateTodoItemDetail(IApplicationDbContext contex) : ICommandHandler<UpdateTodoItemDetailCommand>
{
    public async Task<CrudResult> Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await contex.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            return new CrudResult(CrudStatus.NotFound, "داده ای یافت نشد");

        entity.ListId = request.ListId;
        entity.Priority = request.Priority;
        entity.Note = request.Note;

        await contex.SaveChangesAsync(cancellationToken);

        return new CrudResult(CrudStatus.Succeeded);
    }
}