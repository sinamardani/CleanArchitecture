using Application.Features.TodoLists.Commands.CreateTodoList;
using Application.Features.TodoLists.Commands.DeleteTodoList;
using Application.Features.TodoLists.Commands.PurgeTodoLists;
using Application.Features.TodoLists.Commands.UpdateTodoList;
using Application.Features.TodoLists.Queries.GetTodos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Shared.Models.CustomResult;
using Web.Infrastructure;

namespace Web.Endpoints;

public class TodoLists : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization().MapPost(CreateTodoList, nameof(CreateTodoList));
        groupBuilder.RequireAuthorization().MapPut(UpdateTodoList, nameof(UpdateTodoList));
        groupBuilder.RequireAuthorization().MapGet(GetTodos, nameof(GetTodos));
        groupBuilder.RequireAuthorization().MapDelete(DeleteTodoListBy, nameof(DeleteTodoListBy));
    }

    public async Task<CrudResult<int>> CreateTodoList(ISender sender, CreateTodoListCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<CrudResult> UpdateTodoList(ISender sender, UpdateTodoListCommand command)
    {
        return await sender.Send(command);
    }
    public async Task<CrudResult> DeleteTodoListBy(ISender sender, int id)
    {
        return await sender.Send(new DeleteTodoListCommand(id));
    }
    public async Task<CrudResult> PurgeTodoLists(ISender sender, PurgeTodoListsCommand command)
    {
        return await sender.Send(command);
    }
    public async Task<CrudResult<TodosVm>> GetTodos(ISender sender, [AsParameters] GetTodosQuery query)
    {
        return await sender.Send(query);
    }
}