using Application.Common.Interfaces.Data;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Data.Interceptors;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddServices()
            .AddDatabase<ApplicationDbContext>(configuration);

    private static IServiceCollection AddServices(this IServiceCollection service)
    {
        service.AddScoped<AuditTableEntityInterceptor>();
        service.AddScoped<DispatchDomainEventsInterceptor>();
        service.AddScoped<ApplicationDbContextInitializer>();

        return service;
    }

    private static IServiceCollection AddDatabase<TContext>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>((sp, options) =>
        {
            var auditInterceptor = sp.GetRequiredService<AuditTableEntityInterceptor>();
            var domainEventsInterceptor = sp.GetRequiredService<DispatchDomainEventsInterceptor>();

            options.UseSqlServer(configuration.GetConnectionString("CleanArchitectureDb"))
                .AddInterceptors(auditInterceptor, domainEventsInterceptor);
        });

        services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddHealthChecks()
            .AddDbContextCheck<TContext>(typeof(TContext).Name, tags: new[] { "ready" });

        return services;
    }
}