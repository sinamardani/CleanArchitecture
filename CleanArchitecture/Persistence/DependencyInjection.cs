using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data.Interceptors;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<AuditTableEntityInterceptor>();

        return services;
    }

    public static DbContextOptionsBuilder AddAuditInterceptor(this DbContextOptionsBuilder optionsBuilder, IServiceProvider serviceProvider)
    {
        var interceptor = serviceProvider.GetRequiredService<AuditTableEntityInterceptor>();
        return optionsBuilder.AddInterceptors(interceptor);
    }
}