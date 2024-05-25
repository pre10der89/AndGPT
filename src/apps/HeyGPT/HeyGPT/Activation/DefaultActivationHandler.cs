using Windows.ApplicationModel.Activation;
using HeyGPT.App.Contracts.Services;
using HeyGPT.App.ViewModels;
using Microsoft.Windows.AppLifecycle;

namespace HeyGPT.App.Activation;

public class DefaultActivationHandler : ActivationHandler<AppActivationArguments>
{
    public const string ServiceName = "Default";

    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public override string Name => ServiceName;

    protected override bool CanHandleInternal(AppActivationArguments args)
    {
        // Accessing the "Frame" here requires that we are on the UIThread.
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(AppActivationArguments args)
    {
        object? parameter = null;

        if (args.Data is ILaunchActivatedEventArgs launchActivatedEventArgs)
        {
            parameter = launchActivatedEventArgs.Arguments;
        }

        _navigationService.NavigateTo(typeof(LetsChatViewModel).FullName!, parameter);

        await Task.CompletedTask;
    }
}
