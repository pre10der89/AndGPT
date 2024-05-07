using HeyGPT.App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace HeyGPT.App.Views;

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
