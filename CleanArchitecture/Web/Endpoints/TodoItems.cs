using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Application.TodoItems.Commands.CreateTodoItem;
using Web.Infrastructure;

namespace Web.Endpoints;

public sealed class TodoItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateTodoItem, nameof(CreateTodoItem));
    }

    public async Task<CrudResult<int>> CreateTodoItem(ISender sender, CreateTodoItemCommand command)
    {
        return await sender.Send(command);
    }
}