using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Command;
using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Domain.Common.Enum;
using Microsoft.EntityFrameworkCore;
using System;

namespace CleanArchitecture.Core.Application.TodoLists.Commands.UpdateTodoList;

public sealed record UpdateTodoListCommand : ICommand
{
    public int Id { get; init; }

    public string? Title { get; init; }
}

public sealed class UpdateTodoListCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateTodoListCommand>
{
    public async Task<CrudResult> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.TodoLists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            return new CrudResult(CrudStatus.NotFound, "داده ای یافت نشد");

        entity.Title = request.Title;

        await context.SaveChangesAsync(cancellationToken);
        return new CrudResult(CrudStatus.Succeeded, "با موفقیت ویرایش شد");
    }
}
