using System.Diagnostics;
using HeyGPT.App.Activation;
using HeyGPT.App.Contracts.Services;
using HeyGPT.App.Helpers;
using HeyGPT.App.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

namespace HeyGPT.App.Services;

/// <summary>
/// Manages application activation including initial application launch
/// and advanced activation scenarios such as handling URL protocols.
/// </summary>
public class ActivationService : IActivationService
{
    private readonly IActivationHandler _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly IThemeSelectorService _themeSelectorService;
    private UIElement? _shell;
    private int _activationCount;

    public ActivationService(
        [FromKeyedServices(ActivationHandlerKeys.DefaultServiceKey)] IActivationHandler defaultHandler,
        [FromKeyedServices(ActivationHandlerKeys.OtherServiceKey)] IEnumerable<IActivationHandler> activationHandlers,
        IThemeSelectorService themeSelectorService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _themeSelectorService = themeSelectorService;
    }

    public async Task LaunchAsync(LaunchActivatedEventArgs _)
    {
        // The application is configured to be a singled instance application,
        // which means that we need to handle two types of activation scenarios.
        // When the application is first launched and subsequent activation events
        // such as receiving a URL protocol (e.g. for OAuth redirection). In
        // WindowsAppSDK, you handle the initial launch in the App.OnLaunched override
        // method and secondary activation events through the AppInstance "Activated"
        // event. This method is called from the App.OnLaunched method, which is only
        // called once. 
        //
        // This method will fetch the current AppInstance and subscribe to its Activated
        // event. It will then process the first launch scenario, which will process the
        // main window and initial application startup.  It will also forward the activation
        // event to any additional handlers that are registered.
        //
        // The "Activated" event handler will simply forward the latest activation event
        // to the other interested parties.

        var currentInstance = AppInstance.GetCurrent();

        if (currentInstance is not null && currentInstance.IsCurrent)
        {
            currentInstance.Activated += async (_, arguments) =>
            {
                await HandleSubsequentActivationAsync(arguments).ConfigureAwait(false);
            };

            var appActivationArguments = currentInstance.GetActivatedEventArgs();

            await HandleFirstLaunchAsync(appActivationArguments).ConfigureAwait(false);
        }
    }

    private async Task HandleFirstLaunchAsync(AppActivationArguments activationArgs)
    {
        if (!IsInitialLaunch())
        {
            return;
        }

        IncrementActivationCount();

        // Execute tasks before activation.
        await InitializeAsync();

        // Set the MainWindow Content.
        SetMainWindowContent();

        // Handle activation via ActivationHandlers.
        await HandleActivationAsync(activationArgs);

        // Activate the MainWindow.
        AppWindowHelper.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    private async Task HandleSubsequentActivationAsync(AppActivationArguments args)
    {
        IncrementActivationCount();

        await HandleActivationAsync(args).ConfigureAwait(false);
    }

    private async Task HandleActivationAsync(AppActivationArguments args)
    {
        // NOTE: It isn't guaranteed that this will be executed on the Dispatcher.
        // When the App.OnLaunched is fired, it will be on the Dispatcher, however, 
        // when the AppInstance.Activated is fired it may not be on the Dispatcher.
        // We don't want to force all activation handlers to run on the Dispatcher
        // since there could be many good reasons they can do activity in the background.
        // We also don't necessarily want all activation handlers to be running on whatever
        // thread this method is executing on.  Perhaps, we need two classes of activation
        // handlers (UI & Background). This service would iterate over the list and if it is
        // a background task it is executed on a Task or scheduled to the background.  If it
        // is a UI handler then we queue the handler onto the DispatcherQueue.

        await HandleBackgroundActivationHandlers(args);

        await HandleDefaultActivationHandler(args);
    }

    private async Task HandleBackgroundActivationHandlers(AppActivationArguments args)
    {
        var handlers = _activationHandlers.Where(handler => handler.CanHandle(args));

        foreach (var handler in handlers)
        {
            //await Task.Run(async () =>
            //{
                await RunActivationHandler(handler, args);
            //});
        }
    }

    private async Task RunActivationHandler(IActivationHandler handler, AppActivationArguments args)
    {
        try
        {
            await handler.HandleAsync(args).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to execute activation handler - {ex}");
        }
    }

    private Task HandleDefaultActivationHandler(object activationArgs)
    {
        // This handler needs to be executed on the Dispatcher because it is
        // trying to access the Navigation Service, which is trying to access
        // a UIElement.
        App.MainWindow.DispatcherQueue.TryEnqueue(async () =>
        {
            // TODO: Not sure about this await inside the DispatcherQueue
            // lambda.  Need to think about how to handle activation handlers
            // that need to be run on the dispatcher.  Should it be done
            // internal to the handler itself, which would require each
            // handler to know how to get the Dispatcher.

            if (_defaultHandler.CanHandle(activationArgs))
            {
                await _defaultHandler.HandleAsync(activationArgs);
            }
        });

        AppWindowHelper.Activate();

        return Task.CompletedTask;
    }

    private async Task InitializeAsync()
    {
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        await _themeSelectorService.SetRequestedThemeAsync();
        await Task.CompletedTask;
    }

    private bool IsInitialLaunch()
    {
        // NOTE: Using a simple count might be naive in the case of restarts or other considerations.
        // It progresses us forward right now, will reconsider if problems present themselves...

        return _activationCount == 0;
    }

    private void IncrementActivationCount()
    {
        _activationCount++;
    }

    private void SetMainWindowContent()
    {
        if (AppWindowHelper.HasWindowContent()) { return; }

        _shell = App.GetService<ShellPage>();
        AppWindowHelper.SetWindowContent(_shell);
    }
}
