using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using Microsoft.OpenApi.Models;
using Web.Services;

namespace Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebService(this IServiceCollection service)
    {
        service.AddScoped<IUser, CurrentUser>();
        service.AddHttpContextAccessor();
        service.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>();

        service.AddEndpointsApiExplorer();

        service.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",new OpenApiInfo()
            {
                Version = "V 1.0.0",
                Title = "CleanArchitecture API",
            });
        });
        return service;
    }
}