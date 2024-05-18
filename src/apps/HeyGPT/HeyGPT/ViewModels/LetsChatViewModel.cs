using System.Collections.ObjectModel;
using System.Windows.Input;
using AndGPT.UI.Core.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeyGPT.Core.Contracts.Services;
using HeyGPT.App.Contracts.Services;
using HeyGPT.Core.Models;
using WinUIEx.Messaging;
using Windows.Networking.NetworkOperators;
using AndGPT.Core.Models;

namespace HeyGPT.App.ViewModels;

public partial class LetsChatViewModel : ObservableRecipient
{
    private readonly IOpenAIService _openAIService;
    private readonly ICharacterService _characterService;
    private readonly IClipboardContextService _clipboardContextService;

    public LetsChatViewModel(IOpenAIService openAIService, ICharacterService characterService, IClipboardContextService clipboardContextService)
    {
        _openAIService = openAIService;
        _characterService = characterService;
        _clipboardContextService = clipboardContextService;
        _openAIService.LoginAsync();

        LoadCharacters();
    }

    #region ILetsChatViewModel

    #region Properties

    [ObservableProperty]
    private string? _userMessage;

    [ObservableProperty]
    private string? _messagePlaceholder;

    [ObservableProperty]
    private ChatCharacterViewModel? _selectedChatCharacter;

    [ObservableProperty]
    private bool _shouldIncludeClipboardContext;

    public ObservableCollection<ChatCharacterViewModel> ChatCharacterCollection { get; private set; } = [];

    public ObservableCollection<ChatMessageReceivedViewModel> MessageCollection { get; } = [];

    #endregion

    #region Commands

    #region SendMessageCommand

    private AsyncRelayCommand? _sendMessageCommand;

    public ICommand SendMessageCommand => _sendMessageCommand ??= new AsyncRelayCommand(OnExecuteSendMessageCommand);

    private async Task OnExecuteSendMessageCommand()
    {
        var userMessage = UserMessage?.Trim() ?? string.Empty;

        AddUserMessageToHistory(userMessage);

        try
        {
            var currentCharacterDetails = _characterService.GetSelectedCharacterDetails();

            var extraContext = await GetClipboardContext().ConfigureAwait(true);

            ShouldIncludeClipboardContext = false; // Reset the extra context option after it is used so the user is forced to do it each time.

            var response = await _openAIService.SendPrompt(currentCharacterDetails, userMessage, extraContext.Text).ConfigureAwait(true);

            var message = $"{response.Content} [{response.MetaData.Usage.TotalTokens}]";

            AddRobotMessageToHistory(response.CharacterType, message);
        }
        catch (Exception e)
        {
            AddErrorMessageToHistory(e.ToString());
        }
    }

    #endregion

    #region CharacterSelectedCommand

    private RelayCommand? _characterSelectedCommand;

    public ICommand CharacterSelectedCommand => _characterSelectedCommand ??= new RelayCommand(OnExecuteCharacterSelectedCommand);

    private void OnExecuteCharacterSelectedCommand()
    {
        SelectedChatCharacter ??= _characterService.DefaultCharacter;

        _characterService.SetSelected(SelectedChatCharacter);

        SetMessagePlaceHolder(SelectedChatCharacter.RoleDisplayName);
    }

    #endregion

    #region ToggleShouldIncludeClipboardContextCommand

    private RelayCommand? _toggleShouldIncludeClipboardContextCommand;

    public ICommand ToggleShouldIncludeClipboardContextCommand => _toggleShouldIncludeClipboardContextCommand ??= new RelayCommand(OnExecuteToggleShouldIncludeClipboardContextCommand);

    private void OnExecuteToggleShouldIncludeClipboardContextCommand()
    {
        // TODO: Anything we need to do here;  The boolean value has the desired value already.
    }

    #endregion

    #endregion

    #endregion

    #region Private Methods

    private void LoadCharacters()
    {
        ChatCharacterCollection = new ObservableCollection<ChatCharacterViewModel>(_characterService.Characters);

        SelectedChatCharacter = _characterService.Selected;

        SetMessagePlaceHolder(SelectedChatCharacter.RoleDisplayName);
    }

    private void SetMessagePlaceHolder(string roleDisplayName)
    {
        var formatString = "LetsChat_Enter_Message_TextBoxPlaceholder_Format_Text".GetLocalized();

        MessagePlaceholder = string.Format(formatString, roleDisplayName);
    }

    private async Task<ClipboardText> GetClipboardContext()
    {
        if (!ShouldIncludeClipboardContext)
        {
            return ClipboardText.Empty;
        }

        return await _clipboardContextService.GetContextAsync();
    }

    private void AddUserMessageToHistory(string userMessage)
    {
        if (string.IsNullOrEmpty(userMessage))
        {
            return;
        }

        UserMessage = string.Empty;

        MessageCollection.Add(new ChatMessageReceivedViewModel
        {
            IsActive = true,
            CommunityRole = CharacterType.Local,
            Message = userMessage
        });
    }

    private void AddRobotMessageToHistory(CharacterType characterType, string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        MessageCollection.Add(new ChatMessageReceivedViewModel
        {
            IsActive = true,
            CommunityRole = characterType,
            Message = message
        });
    }

    private void AddErrorMessageToHistory(string errorMessage)
    {
        if (string.IsNullOrEmpty(errorMessage))
        {
            return;
        }

        MessageCollection.Add(new ChatMessageReceivedViewModel
        {
            IsActive = true,
            CommunityRole = CharacterType.Error,
            Message = errorMessage
        });
    }

    #endregion
}
