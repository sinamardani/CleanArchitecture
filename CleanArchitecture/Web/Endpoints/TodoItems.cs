using Application.Features.TodoItems.Commands.CreateTodoItem;
using Application.Features.TodoItems.Commands.DeleteTodoItem;
using Application.Features.TodoItems.Commands.UpdateTodoItem;
using Application.Features.TodoItems.Commands.UpdateTodoItemDetail;
using Application.Features.TodoItems.Queries.GetTodoItemsWithPagination;
using MediatR;
using Shared.Models;
using Shared.Models.CustomResult;
using Web.Infrastructure;

namespace Web.Endpoints;

public class TodoItems : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization().MapPost(CreateTodoItem, nameof(CreateTodoItem));
        groupBuilder.RequireAuthorization().MapPut(UpdateTodoItem, nameof(UpdateTodoItem));
        groupBuilder.RequireAuthorization().MapPut(UpdateTodoItemDetail, nameof(UpdateTodoItemDetail));
        groupBuilder.RequireAuthorization().MapGet(GetTodoItemsWithPagination, nameof(GetTodoItemsWithPagination));
        groupBuilder.RequireAuthorization().MapDelete(DeleteTodoItemBy, nameof(DeleteTodoItemBy));
    }

    public async Task<CrudResult<int>> CreateTodoItem(ISender sender, CreateTodoItemCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<CrudResult> DeleteTodoItemBy(ISender sender, int id)
    {
        return await sender.Send(new DeleteTodoItemCommand(id));
    }
    public async Task<CrudResult> UpdateTodoItem(ISender sender, UpdateTodoItemCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<CrudResult> UpdateTodoItemDetail(ISender sender, UpdateTodoItemDetailCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<CrudResult<PaginatedList<TodoItemBriefDto>>> GetTodoItemsWithPagination(ISender sender, [AsParameters] GetTodoItemsWithPaginationQuery query)
    {
        return await sender.Send(query);
    }
}