using AndGPT.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace AndGPT.WinUI.Views;

public sealed partial class MainWorkspaceControl : UserControl
{
    public MainWorkspaceViewModel ViewModel
    {
        get;
    }

    public MainWorkspaceControl()
    {
        InitializeComponent();

        ViewModel = App.GetService<MainWorkspaceViewModel>();
    }
}
