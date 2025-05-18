using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using Microsoft.OpenApi.Models;
using NSwag;
using Web.Services;
using OpenApiSecurityScheme = NSwag.OpenApiSecurityScheme;

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

        service.AddSwaggerDocument(options =>
        {
            options.Version = "V1.0.0";
            options.Title = "CleanArchitecture API";

            options.AddSecurity("Bearer", new OpenApiSecurityScheme()
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Enter JWT Bearer token **_only_**",
                In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                Name = "Authorization"
            });

            options.OperationProcessors.Add(
                new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("Bearer"));
        });
        return service;
    }
}