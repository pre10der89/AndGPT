using System.Collections.ObjectModel;
using System.Windows.Input;
using AndGPT.UI.Core.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeyGPT.Core.Contracts.Services;

namespace HeyGPT.App.ViewModels;

public partial class HeaderWorkspaceViewModel : ObservableRecipient
{
    private readonly ISampleDataService _sampleDataService;

    public HeaderWorkspaceViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;

        CopyChatLinkCommand = new RelayCommand(OnExecuteCopyChatLinkCommand);
        GptSelectedCommand = new RelayCommand(OnExecuteTransformerSelectedCommand);

        AvailableTransformers = LoadGptOptionsList();

        _selectedTransformer = AvailableTransformers.FirstOrDefault();
    }

    [ObservableProperty]
    private GPTChoiceViewModel? _selectedTransformer;

    public ObservableCollection<GPTChoiceViewModel> AvailableTransformers
    {
        get; private set;
    }

    #region GptSelectedCommand

    public ICommand GptSelectedCommand
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

    private ObservableCollection<GPTChoiceViewModel> LoadGptOptionsList()
    {
        return
        [
            new GPTChoiceViewModel { DisplayName = "AvailableModels_Choice_GPT40".GetLocalized(), TransformerName = "GPT40" },
            new GPTChoiceViewModel { DisplayName = "AvailableModels_Choice_GPT35".GetLocalized(), TransformerName = "GPT35" },
            new GPTChoiceViewModel { DisplayName = "AvailableModels_Choice_AGI".GetLocalized(), TransformerName = "AGI" }
        ];
    }
}
