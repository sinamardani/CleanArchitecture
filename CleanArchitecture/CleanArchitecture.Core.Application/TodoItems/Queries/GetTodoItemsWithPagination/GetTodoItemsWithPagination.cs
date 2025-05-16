using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Command;
using CleanArchitecture.Core.Application.Common.Models;
using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Domain.Common.Enum;
using Mapster;

namespace CleanArchitecture.Core.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public record GetTodoItemsWithPaginationQuery : ICommand<PaginatedList<TodoItemBriefDto>>
{
    public int ListId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTodoItemsWithPaginationQueryHandler(IApplicationDbContext context) : ICommandHandler<GetTodoItemsWithPaginationQuery, PaginatedList<TodoItemBriefDto>>
{
    public async Task<CrudResult<PaginatedList<TodoItemBriefDto>>> Handle(GetTodoItemsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = context.TodoItems
            .Where(x => x.ListId == request.ListId)
            .OrderBy(x => x.Title)
            .ProjectToType<TodoItemBriefDto>();

        var result = await PaginatedList<TodoItemBriefDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);

        return new CrudResult<PaginatedList<TodoItemBriefDto>>(CrudStatus.Succeeded, result);
    }
}
