using CommunityToolkit.Mvvm.ComponentModel;
using HeyGPT.App.Contracts.Services;

namespace HeyGPT.App.ViewModels;

public partial class LoginPageViewModel : ObservableRecipient
{
    private readonly ILoginService _loginService;

    #region Constructor(s)

    public LoginPageViewModel(ILoginService loginService)
    {
        _loginService = loginService;

        _userName = string.Empty;
    }

    #endregion

    #region ILoginPageViewModel Members

    [ObservableProperty] private string _userName;

    #endregion
}
