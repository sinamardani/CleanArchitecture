using CleanArchitecture.Core.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Resources.Service;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure.Resources;

public static class DependencyInjection
{
    public static IServiceCollection AddResourcesInfrastructure(this IServiceCollection service)
    {
        service.AddSingleton<ITranslator, TranslatorService>();

        return service;
    }
}