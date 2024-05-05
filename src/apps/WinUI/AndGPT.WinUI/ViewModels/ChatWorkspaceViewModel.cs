using System.Windows.Input;
using AndGPT.WinUI.Core.Contracts.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AndGPT.WinUI.ViewModels;

public partial class ChatWorkspaceViewModel : ObservableRecipient
{
    private readonly ISampleDataService _sampleDataService;

    public ChatWorkspaceViewModel(ISampleDataService sampleDataService, HeaderWorkspaceViewModel headerWorkspaceViewModel)
    {
        _sampleDataService = sampleDataService;
        _headerWorkspaceViewModel = headerWorkspaceViewModel;

        CopyChatLinkCommand = new RelayCommand(OnExecuteCopyChatLinkCommand);
    }

    [ObservableProperty]
    private HeaderWorkspaceViewModel? _headerWorkspaceViewModel;

    public ICommand CopyChatLinkCommand
    {
        get;
    }

    private void OnExecuteCopyChatLinkCommand()
    {

    }
}
