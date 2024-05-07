using HeyGPTWeb.App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace HeyGPTWeb.App.Views;

public sealed partial class MainPage : Page
{
    public HomePageViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<HomePageViewModel>();
        InitializeComponent();
    }
}
