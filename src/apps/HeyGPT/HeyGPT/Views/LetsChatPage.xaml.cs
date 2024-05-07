using HeyGPT.App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace HeyGPT.App.Views;

public sealed partial class LetsChatPage : Page
{
    public LetsChatViewModel ViewModel
    {
        get;
    }

    public LetsChatPage()
    {
        ViewModel = App.GetService<LetsChatViewModel>();
        InitializeComponent();
    }
}
