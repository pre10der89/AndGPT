using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HeyGPT.App.ViewModels;

public partial class FooterWorkspaceViewModel : ObservableRecipient
{
    public FooterWorkspaceViewModel()
    {
        ShowReleaseNotesCommand = new RelayCommand(OnExecuteShowReleaseNotesCommand);
        ShowHelpCommand = new RelayCommand(OnExecuteShowHelpCommand);
        ShowTermsAndPoliciesCommand = new RelayCommand(OnExecuteShowTermsAndPoliciesCommand);
    }

    #region ShowHelpCommand

    public ICommand ShowHelpCommand
    {
        get;
    }

    private void OnExecuteShowHelpCommand()
    {
    }

    #endregion

    #region ShowReleaseNotesCommand

    public ICommand ShowReleaseNotesCommand
    {
        get;
    }

    private void OnExecuteShowReleaseNotesCommand()
    {
    }

    #region ShowTermsAndPoliciesCommand

    public ICommand ShowTermsAndPoliciesCommand
    {
        get;
    }

    private void OnExecuteShowTermsAndPoliciesCommand()
    {
    }

    #endregion

    #endregion
}
