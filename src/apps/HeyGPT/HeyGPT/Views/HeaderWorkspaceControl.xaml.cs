using HeyGPT.App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace HeyGPT.App.Views;

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
