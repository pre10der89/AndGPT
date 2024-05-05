using AndGPT.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace AndGPT.WinUI.Views;

public sealed partial class FooterWorkspaceControl : UserControl
{
    public FooterWorkspaceViewModel ViewModel
    {
        get;
    }

    public FooterWorkspaceControl()
    {
        InitializeComponent();

        ViewModel = App.GetService<FooterWorkspaceViewModel>();
    }
}
