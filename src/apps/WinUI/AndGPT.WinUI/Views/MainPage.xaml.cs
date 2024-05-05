using System.Collections.ObjectModel;
using AndGPT.WinUI;
using AndGPT.WinUI.ViewModels;
using AndGPT.WinUI.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AndGPT.WinUI.Views;

public sealed partial class MainPage : Page
{
    public SomethingElseViewModel ViewModel
    {
        get;
    }

    private ObservableCollection<NavLink> _navLinks = new ObservableCollection<NavLink>()
    {
        new NavLink() { Label = "People", Symbol = Symbol.People  },
        new NavLink() { Label = "Globe", Symbol = Symbol.Globe },
        new NavLink() { Label = "Message", Symbol = Symbol.Message },
        new NavLink() { Label = "Mail", Symbol = Symbol.Mail },
    };

    public ObservableCollection<NavLink> NavLinks
    {
        get
        {
            return _navLinks;
        }
    }

    public MainPage()
    {
        ViewModel = App.GetService<SomethingElseViewModel>();
        this.InitializeComponent();
    }

    private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
    {
        content.Text = (e.ClickedItem as NavLink).Label + " Page";
    }

    private void PanePlacement_Toggled(object sender, RoutedEventArgs e)
    {
        var ts = sender as ToggleSwitch;
        if (ts.IsOn)
        {
            splitView.PanePlacement = SplitViewPanePlacement.Right;
        }
        else
        {
            splitView.PanePlacement = SplitViewPanePlacement.Left;
        }
    }

    private void displayModeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        splitView.DisplayMode = (SplitViewDisplayMode)Enum.Parse(typeof(SplitViewDisplayMode), (e.AddedItems[0] as ComboBoxItem).Content.ToString());
    }

    private void paneBackgroundCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var colorString = (e.AddedItems[0] as ComboBoxItem).Content.ToString();

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
