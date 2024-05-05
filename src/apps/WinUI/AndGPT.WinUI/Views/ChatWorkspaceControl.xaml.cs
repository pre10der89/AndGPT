using AndGPT.WinUI.Core.Models;
using AndGPT.WinUI.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AndGPT.WinUI.Views;

public sealed partial class ChatWorkspaceControl : UserControl
{
    public ChatWorkspaceViewModel ViewModel
    {
        get;
    }
    
    public SampleOrder? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as SampleOrder;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(SampleOrder), typeof(ChatWorkspaceControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public ChatWorkspaceControl()
    {
        InitializeComponent();

        ViewModel = App.GetService<ChatWorkspaceViewModel>();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        //if (d is ChatWorkspaceControl control)
        //{
        //    control.ForegroundElement.ChangeView(0, 0, 1);
        //}
    }
}
