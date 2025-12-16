using Application.Commons.Interfaces;
using Application.Commons.Interfaces.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddLogService(configuration)
            .AddRedis(configuration);

    private static IServiceCollection AddLogService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ILogService>(sp => new LogService(
            configuration,
            sp.GetRequiredService<IHttpContextAccessor>(),
            //sp.GetRequiredService<ICurrentUserService>(), // Todo CurrentUser
            sp.GetRequiredService<IHostEnvironment>()));
        return services;
    }

    private static IServiceCollection AddRedis(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis");
        
        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });

            services.AddHealthChecks()
                .AddRedis(redisConnectionString, "redis", tags: new[] { "ready" });
        }

        return services;
    }
}

