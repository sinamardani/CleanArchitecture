using Domain.Commons.Constants;
using Domain.Commons.Enums;
using Domain.Identity;
using Domain.TodoItems;
using Domain.TodoLists;
using Domain.TodoLists.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Data;

public class ApplicationDbContextInitializer(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<int>> roleManager,
    ILogger<ApplicationDbContextInitializer> logger)
{
    public async Task InitialiseAsync()
    {
        try
        {
            if (context.Database.IsSqlServer())
            {
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        await SeedRolesAsync();
        await SeedUsersAsync();
        await SeedTodoListsAsync();
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[] { Roles.Administrator, Roles.User };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(role));
                logger.LogInformation("Role {Role} created", role);
            }
        }
    }

    private async Task SeedUsersAsync()
    {
        var administrator = new ApplicationUser
        {
            UserName = "administrator@localhost",
            Email = "administrator@localhost",
            EmailConfirmed = true
        };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Administrator1!");
            await userManager.AddToRolesAsync(administrator, new[] { Roles.Administrator, Roles.User });
            logger.LogInformation("Administrator user created");
        }

        var user = new ApplicationUser
        {
            UserName = "user@localhost",
            Email = "user@localhost",
            EmailConfirmed = true
        };

        if (userManager.Users.All(u => u.UserName != user.UserName))
        {
            await userManager.CreateAsync(user, "User1!");
            await userManager.AddToRoleAsync(user, Roles.User);
            logger.LogInformation("User created");
        }
    }

    private async Task SeedTodoListsAsync()
    {
        if (await context.TodoLists.AnyAsync())
        {
            return;
        }

        var todoList1 = new TodoList
        {
            Title = "Shopping List",
            Colour = Colour.Green
        };

        todoList1.Items.Add(new TodoItem
        {
            Title = "Buy milk",
            Note = "Get 2 liters",
            Priority = PriorityLevel.Medium,
            Done = false
        });

        todoList1.Items.Add(new TodoItem
        {
            Title = "Buy bread",
            Note = "Whole grain preferred",
            Priority = PriorityLevel.Low,
            Done = false
        });

        todoList1.Items.Add(new TodoItem
        {
            Title = "Buy eggs",
            Priority = PriorityLevel.High,
            Done = true
        });

        var todoList2 = new TodoList
        {
            Title = "Work Tasks",
            Colour = Colour.Blue
        };

        todoList2.Items.Add(new TodoItem
        {
            Title = "Complete project documentation",
            Note = "Due by end of week",
            Priority = PriorityLevel.High,
            Done = false
        });

        todoList2.Items.Add(new TodoItem
        {
            Title = "Review code changes",
            Priority = PriorityLevel.Medium,
            Done = false
        });

        var todoList3 = new TodoList
        {
            Title = "Personal",
            Colour = Colour.Purple
        };

        todoList3.Items.Add(new TodoItem
        {
            Title = "Exercise",
            Note = "30 minutes daily",
            Priority = PriorityLevel.Medium,
            Done = false
        });

        context.TodoLists.AddRange(todoList1, todoList2, todoList3);
        await context.SaveChangesAsync();
        logger.LogInformation("Todo lists seeded");
    }
}

