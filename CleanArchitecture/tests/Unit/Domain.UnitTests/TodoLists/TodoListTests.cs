using Domain.TodoLists;
using Domain.TodoLists.ValueObjects;
using FluentAssertions;

namespace Unit.Domain.TodoLists;

public class TodoListTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultColour()
    {
        var todoList = new TodoList
        {
            Title = "Test List"
        };

        todoList.Colour.Should().Be(Colour.White);
        todoList.Items.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_ShouldInitializeWithEmptyItems()
    {
        var todoList = new TodoList
        {
            Title = "Test List"
        };

        todoList.Items.Should().NotBeNull();
        todoList.Items.Should().BeEmpty();
    }

    [Fact]
    public void SetColour_ShouldUpdateColour()
    {
        var todoList = new TodoList
        {
            Title = "Test List",
            Colour = Colour.White
        };

        todoList.Colour = Colour.Red;

        todoList.Colour.Should().Be(Colour.Red);
    }
}

