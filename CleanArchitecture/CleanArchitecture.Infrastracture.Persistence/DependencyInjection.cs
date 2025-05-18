using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using CleanArchitecture.Core.Application.Common.Models;
using CleanArchitecture.Core.Domain.ApplicationRole;
using CleanArchitecture.Core.Domain.ApplicationUsers;
using CleanArchitecture.Infrastructure.Persistence.Data;
using CleanArchitecture.Infrastructure.Persistence.Data.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration) =>
        service
            .AddServices()
            .AddDatabase(configuration);

    public static IServiceCollection AddServices(this IServiceCollection service)
    {
        return service;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection service, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        Ensure.NotNullOrEmpty(connectionString);

        service.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        service.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();


        service.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString).AddAsyncSeeding(sp);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });

        service.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
        service.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        service
            .AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        service.AddScoped<ApplicationDbContextInitializer>();
        return service;
    }
}