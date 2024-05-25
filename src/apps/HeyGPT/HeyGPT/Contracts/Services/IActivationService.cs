using Microsoft.UI.Xaml;

namespace HeyGPT.App.Contracts.Services;

public interface IActivationService
{
    Task LaunchAsync(LaunchActivatedEventArgs args);
}
