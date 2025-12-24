using Application.Common.Interfaces.Data;
using Application.Features.TodoLists.Commands.DeleteTodoList;
using Ardalis.GuardClauses;
using Domain.Commons.Enums;
using Domain.TodoLists;
using Shared.Models.CustomResult;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;

namespace Unit.Application.TodoLists.Commands;

public class DeleteTodoListCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteTodoList_AndReturnSuccessResult()
    {
        var todoList = new TodoList
        {
            Id = 1,
            Title = "Test List"
        };

        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoList>>();

        var data = new List<TodoList> { todoList }.AsQueryable();
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<TodoList>(data.Provider));
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.Expression).Returns(data.Expression);
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        mockContext.Setup(x => x.TodoLists).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.TodoLists.Remove(It.IsAny<TodoList>()));
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new DeleteTodoListCommandHandler(mockContext.Object);
        var command = new DeleteTodoListCommand(1);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
        mockContext.Verify(x => x.TodoLists.Remove(It.IsAny<TodoList>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var mockContext = new Mock<IApplicationDbContext>();
        var mockDbSet = new Mock<DbSet<TodoList>>();

        var data = new List<TodoList>().AsQueryable();
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<TodoList>(data.Provider));
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.Expression).Returns(data.Expression);
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockDbSet.As<IQueryable<TodoList>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        mockContext.Setup(x => x.TodoLists).Returns(mockDbSet.Object);

        var handler = new DeleteTodoListCommandHandler(mockContext.Object);
        var command = new DeleteTodoListCommand(999);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}

internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(System.Linq.Expressions.Expression expression)
    {
        return new TestAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object Execute(System.Linq.Expressions.Expression expression)
    {
        return _inner.Execute(expression)!;
    }

    public TResult Execute<TResult>(System.Linq.Expressions.Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(System.Linq.Expressions.Expression expression)
    {
        return new TestAsyncEnumerable<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(System.Linq.Expressions.Expression expression, CancellationToken cancellationToken)
    {
        var resultType = typeof(TResult).GetGenericArguments()[0];
        var executeMethod = typeof(IQueryProvider)
            .GetMethod(nameof(IQueryProvider.Execute), 1, new[] { typeof(System.Linq.Expressions.Expression) })!
            .MakeGenericMethod(resultType);
        var result = executeMethod.Invoke(_inner, new object[] { expression })!;
        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(resultType)
            .Invoke(null, new[] { result })!;
    }
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(System.Linq.Expressions.Expression expression)
        : base(expression)
    {
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public T Current => _inner.Current;

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(_inner.MoveNext());
    }

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }
}
