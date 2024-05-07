using CommunityToolkit.Mvvm.ComponentModel;
using HeyGPT.Core.Contracts.Services;

namespace HeyGPT.App.ViewModels;

public partial class LetsChatViewModel : ObservableRecipient
{
    private readonly IOpenAIService _openAIService;

    public LetsChatViewModel(IOpenAIService openAIService)
    {
        _openAIService = openAIService;
        _openAIService.InitializeAsync();
    }
}
