using AndGPT.UI.Core.Contracts.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeyGPTWeb.App.Contracts.Services;
using Microsoft.Web.WebView2.Core;

namespace HeyGPTWeb.App.ViewModels;

// TODO: Review best practices and distribution guidelines for WebView2.
// https://docs.microsoft.com/microsoft-edge/webview2/get-started/winui
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/developer-guide
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/distribution
public partial class ChatPageViewModel : ObservableRecipient, INavigationAware
{
    // TODO: Set the default URL to display.
    [ObservableProperty]
    private Uri _source = new(@"https://chatgpt.com/");

    [ObservableProperty]
    private bool _isLoading = true;

    [ObservableProperty]
    private bool _hasFailures;

    public IChatService ChatService
    {
        get;
    }

    public ChatPageViewModel(IChatService chatService)
    {
        ChatService = chatService;
    }

    [RelayCommand]
    private async Task OpenInBrowser()
    {
        if (ChatService.Source != null)
        {
            await Windows.System.Launcher.LaunchUriAsync(ChatService.Source);
        }
    }

    [RelayCommand]
    private void Reload()
    {
        ChatService.Reload();
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoForward))]
    private void BrowserForward()
    {
        if (ChatService.CanGoForward)
        {
            ChatService.GoForward();
        }
    }

    private bool BrowserCanGoForward()
    {
        return ChatService.CanGoForward;
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoBack))]
    private void BrowserBack()
    {
        if (ChatService.CanGoBack)
        {
            ChatService.GoBack();
        }
    }

    private bool BrowserCanGoBack()
    {
        return ChatService.CanGoBack;
    }

    public void OnNavigatedTo(object parameter)
    {
        ChatService.NavigationCompleted += OnNavigationCompleted;
    }

    public void OnNavigatedFrom()
    {
        ChatService.UnregisterEvents();
        ChatService.NavigationCompleted -= OnNavigationCompleted;
    }

    private void OnNavigationCompleted(object? sender, CoreWebView2WebErrorStatus webErrorStatus)
    {
        IsLoading = false;
        BrowserBackCommand.NotifyCanExecuteChanged();
        BrowserForwardCommand.NotifyCanExecuteChanged();

        if (webErrorStatus != default)
        {
            HasFailures = true;
        }
    }

    [RelayCommand]
    private void OnRetry()
    {
        HasFailures = false;
        IsLoading = true;
        ChatService?.Reload();
    }
}
