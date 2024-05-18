using AndGPT.Core.Contracts.Services;
using AndGPT.Core.Services;
using AndGPT.UI.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AndGPT.UI.Core.IoC;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddWindowsInfrastructure(this IServiceCollection services)
    {
        services.TryAddSingleton<IClipboardService, WindowsClipboardService>();

        return services;
    }
}
