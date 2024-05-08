using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeyGPT.Core.Contracts.Services;
using HeyGPT.Core.Models;

namespace HeyGPT.App.ViewModels;

public partial class LetsChatViewModel : ObservableRecipient
{
    private readonly IOpenAIService _openAIService;

    public LetsChatViewModel(IOpenAIService openAIService)
    {
        _openAIService = openAIService;
        _openAIService.InitializeAsync();
    }

    #region ILetsChatViewModel

    #region Properties

    public ObservableCollection<ChatMessageReceivedViewModel> MessageCollection { get; } = [
    
    new ChatMessageReceivedViewModel()
    {
        CommunityRole = new CommunityRole("Pirate"),
        Message = "I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate"
    }, 
    new ChatMessageReceivedViewModel()
    {
        CommunityRole = new CommunityRole("magician"),
        Message = "I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician"
    }, 
    new ChatMessageReceivedViewModel()
    {
        CommunityRole = new CommunityRole("tightropewalker"),
        Message = "I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor"
    }
    ];

    #endregion

    #region Commands

    #region SendMessageCommand

    private AsyncRelayCommand? _sendMessageCommand;

    public ICommand SendMessageCommand => _sendMessageCommand ??= new AsyncRelayCommand(OnExecuteSendMessageCommand, OnCanExecuteSendMessageCommand);

    private bool OnCanExecuteSendMessageCommand()
    {
        return true;
    }

    private async Task OnExecuteSendMessageCommand()
    {
        //var result = await _openAIService.SendTestCompletion().ConfigureAwait(true);
        var result = await _openAIService.SendRealCompletion().ConfigureAwait(true);

        var message = $"{result.Content} [{result.MetaData.Usage.TotalTokens}]";

        MessageCollection.Add(new ChatMessageReceivedViewModel
        {
            IsActive = true,
            CommunityRole = result.CommunityRole,
            Message = message
        });
    }

    #endregion

    #endregion

    #endregion
}
