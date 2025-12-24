using Application.Common.Interfaces.Data;
using Application.Common.Interfaces.Messaging.Query;
using Application.Common.Mappings;
using Domain.Commons.Enums;
using Mapster;
using Shared.Models;
using Shared.Models.CustomResult;

namespace Application.Features.TodoItems.Queries.GetTodoItemsWithPagination;

public record GetTodoItemsWithPaginationQuery : IQuery<PaginatedList<TodoItemBriefDto>>
{
    public int ListId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTodoItemsWithPaginationQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetTodoItemsWithPaginationQuery, PaginatedList<TodoItemBriefDto>>
{
    public async Task<CrudResult<PaginatedList<TodoItemBriefDto>>> Handle(GetTodoItemsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = await context.TodoItems
            .Where(x => x.ListId == request.ListId)
            .OrderBy(x => x.Title)
            .ProjectToType<TodoItemBriefDto>()
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

        return new CrudResult<PaginatedList<TodoItemBriefDto>>(CrudStatus.Succeeded, query);
    }
}
