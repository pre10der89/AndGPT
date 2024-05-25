using HeyGPT.App.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace HeyGPT.App.Helpers;

// TODO: We really should have a service that manages all the application's windows
//       instead of relying on global singletons.  We will almost certainly
//       need to manage the main window and secondary windows and in some cases we'll
//       need to manage them as a group.  For example, if we wanted to minimize
//       the entire application into the "system tray" we'd need to close all windows.
//       And when we want to restore them we'd have to restore them.

internal class AppWindowHelper
{
    public static bool HasWindowContent()
    {
        return App.MainWindow.Content is not null;
    }

    public static void SetWindowContent(UIElement? content)
    {
        if (App.MainWindow.Content == null)
        {
            App.MainWindow.Content = content ?? new Frame();
        }
    }

    public static void Activate()
    {
        if (App.MainWindow.DispatcherQueue is null)
        {
            return;
        }

        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
            App.MainWindow.Activate();
        });
    }

    public static void Restore()
    {
        if (App.MainWindow.DispatcherQueue is null)
        {
            return;
        }

        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
            App.MainWindow.Restore();
        });
    }
}
