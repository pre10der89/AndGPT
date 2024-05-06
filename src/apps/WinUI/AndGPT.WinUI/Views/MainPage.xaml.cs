using System.Collections.ObjectModel;
using AndGPT.WinUI.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace AndGPT.WinUI.Views;

public sealed partial class MainPage : Page
{
    public SomethingElseViewModel ViewModel
    {
        get;
    }

    public ObservableCollection<NavLink> NavLinks
    {
        get;
    } =
    [
        new NavLink() { Label = "People", Symbol = Symbol.People },
        new NavLink() { Label = "Globe", Symbol = Symbol.Globe },
        new NavLink() { Label = "Message", Symbol = Symbol.Message },
        new NavLink() { Label = "Mail", Symbol = Symbol.Mail }
    ];

    public MainPage()
    {
        ViewModel = App.GetService<SomethingElseViewModel>();
        InitializeComponent();
    }

    private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is NavLink navLink)
        {
            content.Text = navLink.Label + " Page";
        }
    }

    private void PanePlacement_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleSwitch ts)
        {
            splitView.PanePlacement = ts.IsOn ? SplitViewPanePlacement.Right : SplitViewPanePlacement.Left;
        }
    }

    private void displayModeCombobox_SelectionChanged(object _, SelectionChangedEventArgs e)
    {
        splitView.DisplayMode = (SplitViewDisplayMode)Enum.Parse(typeof(SplitViewDisplayMode), ((ComboBoxItem)e.AddedItems[0]).Content.ToString() ?? string.Empty);
    }

    private void paneBackgroundCombobox_SelectionChanged(object _, SelectionChangedEventArgs e)
    {
        var colorString = (e.AddedItems[0] as ComboBoxItem)?.Content.ToString();

        VisualStateManager.GoToState(this, colorString, false);
    }
}

public class NavLink
{
    public string Label
    {
        get; set;
    }
    public Symbol Symbol
    {
        get; set;
    }
}
