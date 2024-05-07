using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

namespace HeyGPTWeb.App.Contracts.Services;

public interface IChatService
{
    Uri? Source
    {
        get;
    }

    bool CanGoBack
    {
        get;
    }

    bool CanGoForward
    {
        get;
    }

    event EventHandler<CoreWebView2WebErrorStatus>? NavigationCompleted;

    Task Initialize(WebView2 webView);

    void GoBack();

    void GoForward();

    void Reload();

    void UnregisterEvents();
}
