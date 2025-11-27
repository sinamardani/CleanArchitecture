using Application.Commons.Interfaces.Data;
using Domain.Identity;
using Domain.TodoItems;
using Domain.TodoLists;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Configurations;
using Persistence.Data.Extensions;

namespace Persistence.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options), IApplicationDbContext
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<TodoList> TodoLists => Set<TodoList>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
        modelBuilder.ApplyConfiguration(new TodoListConfiguration());

        modelBuilder.ApplySoftDeleteGlobalQueryFilter();
    }
}
