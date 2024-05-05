using System.Windows.Input;

using AndGPTWeb.Contracts.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

namespace AndGPTWeb.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool _isBackEnabled;

    public ICommand ExitApplicationCommand
    {
        get;
    }

    public ICommand NavigateToSettingsCommand
    {
        get;
    }

    public ICommand NavigateToChatCommand
    {
        get;
    }

    public ICommand NavigateToHomeCommand
    {
        get;
    }

    public INavigationService NavigationService
    {
        get;
    }

    public ShellViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;

        ExitApplicationCommand = new RelayCommand(OnExitApplication);
        NavigateToSettingsCommand = new RelayCommand(OnNavigateToSettings);
        NavigateToChatCommand = new RelayCommand(OnNavigateToChat);
        NavigateToHomeCommand = new RelayCommand(OnNavigateToHome);
    }

    private void OnNavigated(object sender, NavigationEventArgs e) => IsBackEnabled = NavigationService.CanGoBack;

    private void OnExitApplication() => Application.Current.Exit();

    private void OnNavigateToSettings() => NavigationService.NavigateTo(typeof(SettingsViewModel).FullName!);

    private void OnNavigateToChat() => NavigationService.NavigateTo(typeof(ChatPageViewModel).FullName!);

    private void OnNavigateToHome() => NavigationService.NavigateTo(typeof(HomePageViewModel).FullName!);
}
