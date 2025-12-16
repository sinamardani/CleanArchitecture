using Application.Commons.Interfaces;
using Application.Commons.Interfaces.Data;
using Application.Commons.Interfaces.Messaging.Command;
using Application.Commons.Models.CustomResult;
using Domain.Commons.Enums;
using Domain.TodoLists;

namespace Application.TodoLists.Commands.CreateTodoList;

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
