using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Command;
using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Domain.Common.Enum;
using CleanArchitecture.Core.Domain.TodoLists;
using MediatR;

namespace CleanArchitecture.Core.Application.TodoLists.Commands.CreateTodoList;

public sealed record CreateTodoListCommand : ICommand<int>
{
    public string? Title { get; init; }
}

public sealed class CreateTodoListCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateTodoListCommand, int>
{
    public async Task<CrudResult<int>> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoList
        {
            Title = request.Title
        };

        context.TodoLists.Add(entity);

        await context.SaveChangesAsync(cancellationToken);
        return new CrudResult<int>(CrudStatus.Succeeded,"عملیات با موفقیت انجام شد");

    }
}
