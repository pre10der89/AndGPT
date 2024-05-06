using System.Collections.ObjectModel;
using System.Windows.Input;
using AndGPT.UI.Core.Helpers;
using AndGPT.WinUI.Core.Contracts.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AndGPT.WinUI.ViewModels;

public partial class HeaderWorkspaceViewModel : ObservableRecipient
{
    private readonly ISampleDataService _sampleDataService;

    public HeaderWorkspaceViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;

        CopyChatLinkCommand = new RelayCommand(OnExecuteCopyChatLinkCommand);
        TransformerSelectedCommand = new RelayCommand(OnExecuteTransformerSelectedCommand);

        AvailableTransformers = LoadGptOptionsList();

        _selectedTransformer = AvailableTransformers.FirstOrDefault();
    }

    [ObservableProperty]
    private TransformerChoiceViewModel? _selectedTransformer;

    public ObservableCollection<TransformerChoiceViewModel> AvailableTransformers
    {
        get; private set;
    }

    #region TransformerSelectedCommand

    public ICommand TransformerSelectedCommand
    {
        get;
    }

    private void OnExecuteTransformerSelectedCommand()
    {
    }

    #endregion

    #region CopyChatLinkCommand

    public ICommand CopyChatLinkCommand
    {
        get;
    }

    private void OnExecuteCopyChatLinkCommand()
    {
    }

    #endregion

    private ObservableCollection<TransformerChoiceViewModel> LoadGptOptionsList()
    {
        return
        [
            new TransformerChoiceViewModel { DisplayName = "AvailableModels_Choice_GPT40".GetLocalized(), TransformerName = "GPT40" },
            new TransformerChoiceViewModel { DisplayName = "AvailableModels_Choice_GPT35".GetLocalized(), TransformerName = "GPT35" },
            new TransformerChoiceViewModel { DisplayName = "AvailableModels_Choice_AGI".GetLocalized(), TransformerName = "AGI" }
        ];
    }
}
