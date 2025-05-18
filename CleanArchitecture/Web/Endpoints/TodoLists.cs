using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Core.Application.TodoLists.Commands.DeleteTodoList;
using CleanArchitecture.Core.Application.TodoLists.Commands.UpdateTodoList;
using CleanArchitecture.Core.Application.TodoLists.Queries.GetTodos;
using Web.Infrastructure;

namespace Web.Endpoints;

public sealed class TodoLists : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateTodoList, nameof(CreateTodoList))
            .MapDelete(DeleteTodoList, nameof(DeleteTodoList))
            .MapPut(UpdateTodoList, nameof(UpdateTodoList))
            .MapGet(GetTodoLists, nameof(GetTodoLists));
    }

    public async Task<CrudResult<int>> CreateTodoList(ISender sender, CreateTodoListCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<CrudResult> DeleteTodoList(ISender sender, int id)
    {
        return await sender.Send(new DeleteTodoListCommand(id));
    }

    public async Task<CrudResult> UpdateTodoList(ISender sender, UpdateTodoListCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<CrudResult<TodosVm>> GetTodoLists(ISender sender)
    {
        return await sender.Send(new GetTodosQuery());
    }
}