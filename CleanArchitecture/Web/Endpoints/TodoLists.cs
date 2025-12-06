using Application.Commons.Models.CustomResult;
using Application.TodoLists.Commands.CreateTodoList;
using Application.TodoLists.Commands.DeleteTodoList;
using Application.TodoLists.Commands.PurgeTodoLists;
using Application.TodoLists.Commands.UpdateTodoList;
using Application.TodoLists.Queries.GetTodos;
using MediatR;
using Web.Infrastructure;

namespace Web.Endpoints;

public class TodoLists : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateTodoList, nameof(CreateTodoList));
        groupBuilder.MapPut(UpdateTodoList, nameof(UpdateTodoList));
        groupBuilder.MapGet(GetTodos, nameof(GetTodos));
        groupBuilder.MapDelete(DeleteTodoListBy, nameof(DeleteTodoListBy));
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