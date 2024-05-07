using AndGPT.Core.Contracts.Services;
using AndGPT.Core.Services;
using HeyGPT.Core.Contracts.Services;
using HeyGPT.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HeyGPT.Core.IoC;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddOpenAI(this IServiceCollection services)
    {
        services.TryAddSingleton<IOpenAIService, OpenAIService>();

        return services;
    }
}
