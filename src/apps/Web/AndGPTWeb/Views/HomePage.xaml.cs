using AndGPTWeb.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace AndGPTWeb.Views;

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
