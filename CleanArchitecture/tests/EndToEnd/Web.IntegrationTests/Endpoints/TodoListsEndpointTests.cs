using System.Net;
using System.Net.Http.Json;
using Application.Features.TodoLists.Commands.CreateTodoList;
using Application.Features.TodoLists.Queries.GetTodos;
using FluentAssertions;
using Shared.Models.CustomResult;
using Web.IntegrationTests;

namespace EndToEnd.Web.Endpoints;

public class TodoListsEndpointTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TodoListsEndpointTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
    }

    [Fact]
    public async Task CreateTodoList_ShouldReturnSuccess()
    {
        var command = new CreateTodoListCommand
        {
            Title = "Test List"
        };

        var response = await _client.PostAsJsonAsync("/api/TodoLists", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<CrudResult<int>>();
        result.Should().NotBeNull();
        result!.Status.Should().Be(Domain.Commons.Enums.CrudStatus.Succeeded);
    }

    [Fact]
    public async Task GetTodos_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("/api/TodoLists");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<CrudResult<Application.Features.TodoLists.Queries.GetTodos.TodosVm>>();
        result.Should().NotBeNull();
        result!.Status.Should().Be(Domain.Commons.Enums.CrudStatus.Succeeded);
    }
}

