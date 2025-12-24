using Application.Common.Interfaces.Data;
using Application.Common.Interfaces.Messaging.Command;
using Domain.Commons.Enums;
using Domain.TodoLists;
using Shared.Interfaces;
using Shared.Models.CustomResult;

namespace Application.Features.TodoLists.Commands.CreateTodoList;

public record CreateTodoListCommand : ICommand<int>
{
    public string? Title { get; init; }
}

public class CreateTodoListCommandHandler(IApplicationDbContext context,ILogService logger) : ICommandHandler<CreateTodoListCommand, int>
{
    public async Task<CrudResult<int>> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoList
        {
            Title = request.Title
        };

        context.TodoLists.Add(entity);

        await context.SaveChangesAsync(cancellationToken);
        logger.DbLog("CreateTodoListCommandHandler Successfully");
        return new CrudResult<int>(CrudStatus.Succeeded, entity.Id);
    }
}
