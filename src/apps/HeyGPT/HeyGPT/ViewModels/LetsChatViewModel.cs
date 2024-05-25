using System.Collections.ObjectModel;
using System.Windows.Input;
using AndGPT.Core.Contracts.Services;
using AndGPT.UI.Core.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeyGPT.Core.Contracts.Services;
using HeyGPT.App.Contracts.Services;
using HeyGPT.Core.Models;
using AndGPT.Core.Models;
using AndGPT.Core.Events;
using Microsoft.UI.Dispatching;
using System.Text;
using HeyGPT.App.Models;
using System;

namespace HeyGPT.App.ViewModels;

public partial class LetsChatViewModel : ObservableRecipient
{
    private readonly IOpenAIService _openAIService;
    private readonly ICharacterService _characterService;
    private readonly IClipboardContextService _clipboardContextService;
    private readonly IFileService _fileService;
    private readonly IEventAggregator _eventAggregator;

    public LetsChatViewModel(
        IOpenAIService openAIService,
        ICharacterService characterService,
        IClipboardContextService clipboardContextService,
        IFileService fileService,
        IEventAggregator eventAggregator)
    {
        _openAIService = openAIService;
        _characterService = characterService;
        _clipboardContextService = clipboardContextService;
        _fileService = fileService;
        _eventAggregator = eventAggregator;
        _openAIService.LoginAsync();

        LoadCharacters();

        SubscribeToEvents();
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

        await SendMessageInternal(userMessage);

        //AddUserMessageToHistory(userMessage);

        //try
        //{
        //    var currentCharacterDetails = _characterService.GetSelectedCharacterDetails();

        //    var extraContext = await GetClipboardContext().ConfigureAwait(true);

        //    ShouldIncludeClipboardContext = false; // Reset the extra context option after it is used so the user is forced to do it each time.

        //    var response = await _openAIService.SendPrompt(currentCharacterDetails, userMessage, extraContext.Text).ConfigureAwait(true);

        //    var message = $"{response.Content} [{response.MetaData.Usage.TotalTokens}]";

        //    AddRobotMessageToHistory(response.CharacterType, message);
        //}
        //catch (Exception e)
        //{
        //    AddErrorMessageToHistory(e.ToString());
        //}
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

    #region Event Handlers

    private void SubscribeToEvents()
    {
        _eventAggregator.Subscribe<FileActivatedEvent>(OnFileActivatedEvent);
        _eventAggregator.Subscribe<ProtocolActivatedEvent>(OnProtocolActivatedEvent);
    }

    private void OnFileActivatedEvent(FileActivatedEvent fileActivatedEvent)
    {
        var firstFile = fileActivatedEvent.Files[0];

        var messagePrompt = GetMessagePromptFromFile(firstFile);

        if (messagePrompt is null)
        {
            return;
        }

        var message = messagePrompt.Value.Message;
        var character = GetChatCharacterDetailsFromString(messagePrompt.Value.Character);

        if (!string.IsNullOrWhiteSpace(message))
        {
            App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, async () =>
            {
                await SendMessageInternal(message, character);
            });
        }
    }


    private ChatCharacterDetails GetChatCharacterDetailsFromString(string character)
    {
        var characterType = !string.IsNullOrEmpty(character) ? new CharacterType(character) : CharacterType.Default;
        
        return _characterService.GetCharacter(characterType);
    }
    private void OnProtocolActivatedEvent(ProtocolActivatedEvent protocolActivatedEvent)
    {
        if (protocolActivatedEvent.Protocol != "hey")
        {
            return;
        }

        try
        {
            var parameters = ParseHeyProtocolUri(protocolActivatedEvent.Uri);

            var message = parameters.Message;
            var character = GetChatCharacterDetailsFromString(parameters.Character);

            if (!string.IsNullOrWhiteSpace(message))
            {
                App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, async () =>
                {
                    await SendMessageInternal(message, character);
                });
            }
        }
        catch (Exception e)
        {
            AddErrorMessageToHistory($"Protocol parsing error - {e.Message}");
        }
    }

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

    private async Task SendMessageInternal(string userMessage, ChatCharacterDetails? desiredCharacterDetails = null)
    {
        AddUserMessageToHistory(userMessage);

        try
        {
            var characterDetails = desiredCharacterDetails ?? _characterService.GetSelectedCharacterDetails();

            var extraContext = await GetClipboardContext().ConfigureAwait(true);

            ShouldIncludeClipboardContext = false; // Reset the extra context option after it is used so the user is forced to do it each time.

            var response = await _openAIService.SendPrompt(characterDetails, userMessage, extraContext.Text).ConfigureAwait(true);

            var message = $"{response.Content} [{response.MetaData.Usage.TotalTokens}]";

            AddRobotMessageToHistory(response.CharacterType, message);
        }
        catch (Exception e)
        {
            AddErrorMessageToHistory(e.ToString());
        }
    }

    public static (string Character, string Message) ParseHeyProtocolUri(Uri uri)
    {
        // Ensure the URI is in the correct format
        if (uri.Scheme != "hey" || !uri.Host.Equals("send", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("The URI is not in the correct format.");
        }

        // Get the segments of the URI
        var segments = uri.Segments;

        if (segments.Length < 3)
        {
            throw new ArgumentException("The URI does not contain enough segments.");
        }

        // Extract the Character and Base64 encoded message from the segments
        var character = segments[1].Trim('/');
        var base64Message = segments[2];

        // Decode the Base64 message
        var message = Encoding.UTF8.GetString(Convert.FromBase64String(base64Message));

        return (character, message);
    }

    private static readonly Random Random = new();

    public static int GetRandomIndex(int lowerBound, int upperBound)
    {
        if (lowerBound > upperBound)
        {
            throw new ArgumentException("Lower bound must be less than or equal to upper bound.");
        }

        return Random.Next(lowerBound, upperBound + 1);
    }

    private SimpleMessagePrompt? GetMessagePromptFromFile(ActivatedFile file)
    {
        var fromStructuredFile = GetRandomPromptFromStructureFile(file);

        return fromStructuredFile ?? GetMessagePromptFromUnstructuredFile(file);
    }

    private SimpleMessagePrompt? GetRandomPromptFromStructureFile(ActivatedFile file)
    {
        try
        {
            var promptCollection = GetMessagePromptCollectionFromFile(file);

            if (promptCollection is null || promptCollection.Count == 0)
            {
                return null;
            }

            var promptIndex = GetRandomIndex(0, promptCollection.Count - 1);

            return promptCollection[promptIndex];
        }
        catch
        {
            // Ignored
        }

        return null;
    }

    private SimpleMessagePrompt? GetMessagePromptFromUnstructuredFile(ActivatedFile file)
    {
        try
        {
            var message = _fileService.ReadAllText(file.Path);

            return new SimpleMessagePrompt
            {
                Character = "default",
                Message = message
            };
        }
        catch
        {
            // Ignored
        }

        return null;
    }

    private SimpleMessagePromptCollection? GetMessagePromptCollectionFromFile(ActivatedFile file)
    {
        try
        {
            var fileParts = IFileService.GetDirectoryAndFileName(file.Path);

            return _fileService.Read<SimpleMessagePromptCollection>(fileParts.directory, fileParts.fileName);
        }
        catch
        {
            // Ignored
        }

        return null;
    }

    #endregion
}
