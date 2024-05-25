using AndGPT.Core.Contracts.Services;
using AndGPT.Core.Ioc;
using AndGPT.Core.Services;
using AndGPT.UI.Core.IoC;
using HeyGPT.App.Activation;
using HeyGPT.App.Contracts.Policies;
using HeyGPT.App.Contracts.Services;
using HeyGPT.App.Models;
using HeyGPT.App.Policies;
using HeyGPT.App.Services;
using HeyGPT.App.ViewModels;
using HeyGPT.App.Views;
using HeyGPT.Core.Contracts.Services;
using HeyGPT.Core.IoC;
using HeyGPT.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace HeyGPT.App;

public partial class App : Application
{
    public IHost Host { get; }

    // TODO: We probably want to manage the MainWindow in a dedicated service.
    // Currently, anybody that needs access to the MainWindow will need to use the
    // App global object. We also may want to handle showing/hiding the main window
    // as well as multiple child windows.  And we may want to respond on events on the
    // MainWindow.
    // Currently, there is some coupling with the AppTitleBar helpers and other places
    // where global access to the App object is being used.  Those need to be settled first.
    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices((context, services) =>
            {
                // Activation Handler(s)

                // Default Activation Handler
                services.AddKeyedTransient<IActivationHandler, DefaultActivationHandler>(ActivationHandlerKeys.DefaultServiceKey);
                // Other Activation Handlers
                services.AddKeyedTransient<IActivationHandler, ProtocolActivationHandler>(ActivationHandlerKeys.OtherServiceKey);
                services.AddKeyedTransient<IActivationHandler, FileTypeActivationHandler>(ActivationHandlerKeys.OtherServiceKey);

                // Services
                services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
                services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
                services.AddTransient<INavigationViewService, NavigationViewService>();

                services.AddSingleton<IEventAggregator, EventAggregator>();
                services.AddSingleton<IActivationService, ActivationService>();
                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<INavigationService, NavigationService>();

                // Core Services
                services.AddSingleton<ISampleDataService, SampleDataService>();
                services.AddSingleton<ILoginService, LoginService>();
                services.AddInfrastructure();
                services.AddWindowsInfrastructure();
                services.AddOpenAI();

                // Application Services
                services.AddSingleton<ICharacterService, CharacterService>();
                services.AddSingleton<IClipboardContextPolicy, ClipboardContextPolicy>();
                services.AddSingleton<IClipboardContextService, ClipboardContextService>();

                // Views and ViewModels
                services.AddTransient<LoginPageViewModel>();
                services.AddTransient<SettingsViewModel>();
                services.AddTransient<SettingsPage>();
                services.AddTransient<DataGridViewModel>();
                services.AddTransient<DataGridPage>();
                services.AddTransient<SampleDataViewModel>();
                services.AddTransient<SampleDataPage>();
                services.AddTransient<LetsChatViewModel>();
                services.AddTransient<LetsChatPage>();
                services.AddTransient<ShellPage>();
                services.AddTransient<ShellViewModel>();
                services.AddTransient<HeaderWorkspaceViewModel>();
                services.AddTransient<FooterWorkspaceViewModel>();

                // Configuration
                services.Configure<LocalSettingsOptions>(
                    context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            }).Build();

        UnhandledException += App_UnhandledException;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        // NOTE: When the application is first launched, the bootstrapping process will
        // call this method with a "Kind" defining how it was launched.
        // If the user double-clicked on the icon the "Kind" would be "Launch" and if
        // the application was launched via a URL Protocol then the "Kind" will be
        // "Protocol".  There are many other "Kind" values that are possible.
        //
        // By default, WindowsAppSDK applications allow multiple instances to run
        // simultaneously. In this mode, each new instance of the application will
        // fire this method with the appropriate "Kind".
        //
        // This application, however, is set up to be a single instance application meaning
        // that only one instance is allowed to persist at a time. It is possible to launch a
        // new instance for utility purposes, however, the second instance will either
        // defer to the first instance or perform some action and exit.
        // The "Main" method of the second instance will detect that another instance
        // with the same "AppKey" is already running, and it can forward the activation
        // information to the first instance.
        //
        // In the single instance setup, the "App.OnLaunched" method will only be called
        // when the first instance is launched. It will NOT be called for any additional
        // instances. Subsequent activation notifications will be broadcast via the
        // Microsoft.Windows.AppLifecycle.AppInstance.Activated event. This event
        // will be fired for all activation events after the initial launch event.
        // 
        // The "LaunchActivatedEventArgs" are passed to the Activation Service, which
        // handles the initial launch request and any subsequent activation events.
        // It manages the initialization of the Shell in the main window on the initial
        // launch and main window activation on subsequent events.

        await App.GetService<IActivationService>().LaunchAsync(args);
    }
}
