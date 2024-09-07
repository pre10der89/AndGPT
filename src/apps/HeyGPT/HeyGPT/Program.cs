// If you need to attach the debugger early when you are not running in the IDE then
// you can uncomment the #define WAIT_FOR_DEBUGGER_TO_ATTACH. This will display a message box
// pausing execution of the application allowing you to attach the debugger.  Dismiss the message
// box and program execution will continue.

// #define WAIT_FOR_DEBUGGER_TO_ATTACH

#nullable disable

using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;
using WinRT;
using System.Diagnostics;
using HeyGPT.App.Helpers;

namespace HeyGPT.App;

public class Program
{
    private const string AppKey = "HeyGPT_3AFC048E-1B59-48D4-8DC0-A98E952482F0";

    // We override the default Main function to enforce single instance mechanisms.
    // We need single instance support to allow us to handle URI protocols needed
    // for communication with other process such as web browsers when doing OAuth
    // redirection.  It also allows us to trigger the application to the foreground
    // when the user tries to run the application a second time.

    [STAThread]
    private static async Task<int> Main(string[] args)
    {
        // See above
        DebuggerHelper.WaitForDebuggerToAttach();

        ComWrappersSupport.InitializeComWrappers();

        var isRedirect = await DecideRedirection().ConfigureAwait(false);

        if (!isRedirect)
        {
            // This is the first instance of the application, so we start the application
            // object.

            Microsoft.UI.Xaml.Application.Start((p) =>
            {
                var context = new DispatcherQueueSynchronizationContext(
                    DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);
                new App();
            });

            // The application has been closed and is about the exit.
            // We clean up here...

            AppInstance.GetCurrent().UnregisterKey();
        }

        return 0;
    }

    private static async Task<bool> DecideRedirection()
    {
        var isRedirect = false;

        try
        {
            var appActivationArguments = AppInstance.GetCurrent().GetActivatedEventArgs();
            var extendedActivationKind = appActivationArguments.Kind;

            var appInstance = AppInstance.FindOrRegisterForKey(AppKey);

            // Handle any activation scenario that should be processed before the
            // application starts.
            HandleActivation(appActivationArguments);


            if (appInstance.IsCurrent)
            {
                // We have successfully registered the app key, this must be the
                // only instance running that has been activated with that app key.

                Console.WriteLine(@$"Application was launched with ProcessId=[{Environment.ProcessId}] activated with Kind[{appActivationArguments.Kind}]");
            }
            else
            {
                Console.WriteLine(@$"Second instance of this application was launched with ProcessId=[{Environment.ProcessId}] activated with Kind[{appActivationArguments.Kind}]. Forwarding activation to the first instance.");

                isRedirect = true;

                // Ensure we don't block the STA, by doing the redirect operation in another thread.
                await Task.Run(async () =>
                {
                    await appInstance.RedirectActivationToAsync(appActivationArguments);
                }).ConfigureAwait(true);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Application start redirection failed: {ex.Message}");

            Environment.FailFast("Application start redirection failed", ex);
        }

        return isRedirect;
    }

    private static bool HandleActivation(AppActivationArguments args)
    {
        return true; // Should continue
    }

}
