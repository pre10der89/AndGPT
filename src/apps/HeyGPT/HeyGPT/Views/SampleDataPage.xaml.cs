using CommunityToolkit.WinUI.UI.Controls;
using HeyGPT.App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace HeyGPT.App.Views;

public sealed partial class SampleDataPage : Page
{
    public SampleDataViewModel ViewModel
    {
        get;
    }

    public SampleDataPage()
    {
        ViewModel = App.GetService<SampleDataViewModel>();
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
