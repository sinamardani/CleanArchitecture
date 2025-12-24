using Shared.Models.AppSettings;
using Infrastructure.Services;
using Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Interfaces;
using Shared.Interfaces.Authentication;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddService(configuration)
            .AddLogService(configuration)
            .AddRedis(configuration);

    private static IServiceCollection AddLogService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ILogService>(sp => new LogService(
            configuration,
            sp.GetRequiredService<IHttpContextAccessor>(),
            sp.GetRequiredService<ICurrentUserService>(),
            sp.GetRequiredService<IHostEnvironment>()));
        return services;
    }

    private static IServiceCollection AddService(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        service.AddScoped<IJwtService, JwtService>();

        return service;
    }

    private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
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

