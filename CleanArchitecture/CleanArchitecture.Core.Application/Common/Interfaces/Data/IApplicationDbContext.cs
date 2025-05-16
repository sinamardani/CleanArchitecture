using CleanArchitecture.Core.Domain.TodoItems;
using CleanArchitecture.Core.Domain.TodoLists;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Core.Application.Common.Interfaces.Data;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}