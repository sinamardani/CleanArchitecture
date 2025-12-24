using Application.Common.Interfaces.Data;
using Application.Common.Interfaces.Messaging.Command;
using Domain.Commons.Enums;
using Domain.TodoItems;
using Domain.TodoItems.Events;
using Shared.Models.CustomResult;

namespace Application.Features.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand : ICommand<int>
{
    public int ListId { get; init; }

    public string? Title { get; init; }
}

public class CreateTodoItemCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreateTodoItemCommand, int>
{
    public async Task<CrudResult<int>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Done = false
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        context.TodoItems.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return new CrudResult<int>(CrudStatus.Succeeded, entity.Id);
    }
}
