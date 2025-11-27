using System.Reflection;
using Application.Commons.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors;
using Web.Commons.Services;

namespace Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services) =>
        services
            .AddServices()
            .AddSwagger();

    private static IServiceCollection AddServices(this IServiceCollection service)
    {
        return service
            .AddHttpContextAccessor()
            .AddScoped<ICurrentUserService, CurrentUserService>();
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