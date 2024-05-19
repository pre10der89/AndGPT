using AndGPT.Core.Contracts.Services;
using AndGPT.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AndGPT.Core.Ioc;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.TryAddSingleton<IFileService, FileService>();
        services.TryAddSingleton<IEnvironmentVariableService, WindowsEnvironmentVariableService>();

        return services;
    }
}
