using CleanArchitecture.Core.Application.Common.Models;
using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Core.Application.TodoItems.Commands.DeleteTodoItem;
using CleanArchitecture.Core.Application.TodoItems.Commands.UpdateTodoItem;
using CleanArchitecture.Core.Application.TodoItems.Commands.UpdateTodoItemDetail;
using CleanArchitecture.Core.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using Web.Infrastructure;

namespace Web.Endpoints;

public sealed class TodoItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            //.RequireAuthorization()
            .MapPost(CreateTodoItem, nameof(CreateTodoItem))
            .MapPut(UpdateTodoItem,nameof(UpdateTodoItem))
            .MapPut(UpdateTodoItemDetail,nameof(UpdateTodoItemDetail))
            .MapDelete(DeleteTodoItem,nameof(DeleteTodoItem))
            .MapGet(GetTodoItemsWithPagination,nameof(GetTodoItemsWithPagination));
    }

    public async Task<CrudResult<int>> CreateTodoItem(ISender sender, CreateTodoItemCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<CrudResult> UpdateTodoItem(ISender sender, UpdateTodoIemCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<CrudResult> DeleteTodoItem(ISender sender,int id)
    {
        return await sender.Send(new DeleteTodoItemCommand(id));
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