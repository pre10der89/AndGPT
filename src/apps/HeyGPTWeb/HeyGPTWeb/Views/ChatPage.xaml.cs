using HeyGPTWeb.App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace HeyGPTWeb.App.Views;

// To learn more about WebView2, see https://docs.microsoft.com/microsoft-edge/webview2/.
public sealed partial class ChatPage : Page
{
    public ChatPageViewModel ViewModel
    {
        get;
    }

    public ChatPage()
    {
        ViewModel = App.GetService<ChatPageViewModel>();
        InitializeComponent();

        Loaded += async (sender, args) =>
        {
            await ViewModel.ChatService.Initialize(WebView);
        };
    }
}
