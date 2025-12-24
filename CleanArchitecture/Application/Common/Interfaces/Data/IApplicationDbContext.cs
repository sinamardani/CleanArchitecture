using Domain.TodoItems;
using Domain.TodoLists;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces.Data;

public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; }
    DbSet<TodoList> TodoLists { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

