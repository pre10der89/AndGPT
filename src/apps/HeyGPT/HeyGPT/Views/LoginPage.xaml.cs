using HeyGPT.App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace HeyGPT.App.Views;

public sealed partial class LoginPage : Page
{
    public LoginPageViewModel ViewModel
    {
        get;
    }

    public LoginPage()
    {
        ViewModel = App.GetService<LoginPageViewModel>();
        InitializeComponent();
    }
}
