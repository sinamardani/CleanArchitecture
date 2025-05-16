using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Command;
using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Domain.Common.Enum;
using CleanArchitecture.Core.Domain.TodoItems;
using CleanArchitecture.Core.Domain.TodoItems.Events;

namespace CleanArchitecture.Core.Application.TodoItems.Commands.CreateTodoItem;

public sealed record CreateTodoItemCommand : ICommand<int>
{
    public int ListId { get; init; }
    public string? Title { get; init; }
}

public sealed class CreateTodoItemCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateTodoItemCommand,int>
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

        return new CrudResult<int>(CrudStatus.Succeeded, "Todo شما با موفقیت ثبت شد");
    }
}