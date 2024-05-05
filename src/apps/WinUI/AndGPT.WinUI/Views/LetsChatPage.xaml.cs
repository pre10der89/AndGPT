using AndGPT.WinUI.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace AndGPT.WinUI.Views;

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

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
