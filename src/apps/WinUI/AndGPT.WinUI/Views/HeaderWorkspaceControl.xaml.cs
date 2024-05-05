using AndGPT.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace AndGPT.WinUI.Views;

public sealed partial class HeaderWorkspaceControl : UserControl
{
    public HeaderWorkspaceViewModel ViewModel
    {
        get;
    }

    public HeaderWorkspaceControl()
    {
        InitializeComponent();

        ViewModel = App.GetService<HeaderWorkspaceViewModel>();
    }
}
