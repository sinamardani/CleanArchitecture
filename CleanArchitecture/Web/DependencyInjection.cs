using System.Reflection;
using System.Security.Cryptography;
using Application.Commons.Models.AppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors;
using Application.Commons.Interfaces.Data;
using Web.Services;

namespace Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment) =>
        services
            .AddServices()
            .AddAuthentication(configuration, environment)
            .AddSwagger();

    private static IServiceCollection AddServices(this IServiceCollection service)
    {
        return service
            .AddHttpContextAccessor()
            .AddScoped<ICurrentUserService, CurrentUserService>();
    }

    private static IServiceCollection AddAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        
        if (jwtSettings == null)
            throw new InvalidOperationException("JwtSettings configuration is missing");

        var publicKeyPath = Path.Combine(environment.ContentRootPath, jwtSettings.PublicKeyPath);
        
        if (!File.Exists(publicKeyPath))
            throw new FileNotFoundException($"Public key file not found: {publicKeyPath}");

        var publicKeyContent = File.ReadAllText(publicKeyPath);
        var publicKey = ECDsa.Create();
        publicKey.ImportFromPem(publicKeyContent);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new ECDsaSecurityKey(publicKey),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection service)
    {
        return service
            .AddEndpointsApiExplorer()
            .AddOpenApiDocument(option =>
            {
                option.Version = "V1.0.0";
                option.Title = "CleanArchitecture API";
                option.OperationProcessors.Add(new FormDataOperationProcessor());
            });
    }

    private class FormDataOperationProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            var hasConsumesMultipartAttr = context.MethodInfo?
                .GetCustomAttributes(typeof(ConsumesAttribute), inherit: true)
                .Cast<ConsumesAttribute>()
                .Any(attr => attr.ContentTypes.Contains("multipart/form-data")) ?? false;

            if (!hasConsumesMultipartAttr)
                return true;

            var modelParam = context.MethodInfo?.GetParameters()
                .FirstOrDefault(p => p.GetCustomAttribute<FromFormAttribute>() != null);

            if (modelParam is null)
                return true;

            var modelType = modelParam.ParameterType;

            var schema = context.SchemaGenerator.Generate(modelType, context.SchemaResolver);

            context.OperationDescription.Operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = schema
                    }
                },
                IsRequired = true
            };

            return true;
        }
    }
}