using Application.Commons.Models;
using Application.Commons.Models.CustomResult;
using Application.TodoItems.Commands.CreateTodoItem;
using Application.TodoItems.Commands.DeleteTodoItem;
using Application.TodoItems.Commands.UpdateTodoItem;
using Application.TodoItems.Commands.UpdateTodoItemDetail;
using Application.TodoItems.Queries.GetTodoItemsWithPagination;
using MediatR;
using Web.Infrastructure;

namespace Web.Endpoints;

public class TodoItems : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateTodoItem, nameof(CreateTodoItem));
        groupBuilder.MapPut(UpdateTodoItem, nameof(UpdateTodoItem));
        groupBuilder.MapPut(UpdateTodoItemDetail, nameof(UpdateTodoItemDetail));
        groupBuilder.MapGet(GetTodoItemsWithPagination, nameof(GetTodoItemsWithPagination));
        groupBuilder.MapDelete(DeleteTodoItemBy, nameof(DeleteTodoItemBy));
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