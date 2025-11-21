using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors;

namespace Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services) =>
        services
            .AddSwagger();

    private static IServiceCollection AddSwagger(this IServiceCollection service)
    {
        service.AddEndpointsApiExplorer();

        service.AddOpenApiDocument(option =>
        {
            option.Version = "V1.0.0";
            option.Title = "CleanArchitecture API";
            option.OperationProcessors.Add(new FormDataOperationProcessor());
        });

        return service;
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