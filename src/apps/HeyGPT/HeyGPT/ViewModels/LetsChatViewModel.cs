using System.Collections.ObjectModel;
using System.Windows.Input;
using AndGPT.UI.Core.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeyGPT.Core.Contracts.Services;
using HeyGPT.Core.Models;
using Windows.ApplicationModel.DataTransfer;

namespace HeyGPT.App.ViewModels;

public partial class LetsChatViewModel : ObservableRecipient
{
    private readonly IOpenAIService _openAIService;

    private CommunityMember _currentCommunityMember;

    private bool _sendClipboardContentWithNextCompletion;
    private string _lastClipboardContent;

    public LetsChatViewModel(IOpenAIService openAIService)
    {
        _openAIService = openAIService;
        _openAIService.InitializeAsync();

        LoadCommunityMembers();
    }

    #region ILetsChatViewModel

    #region Properties

    [ObservableProperty]
    private string? _userMessage;

    [ObservableProperty]
    private string? _messagePlaceholder;

    [ObservableProperty]
    private CommunityMemberViewModel? _selectedCommunityMember;

    [ObservableProperty] 
    private bool _isMonitoringClipboard;

    public ObservableCollection<CommunityMemberViewModel> CommunityMemberCollection { get; private set; } = [];

    public ObservableCollection<ChatMessageReceivedViewModel> MessageCollection
    {
        get;
    } = [

    //new ChatMessageReceivedViewModel
    //{
    //    CommunityRole = new CommunityRole("Pirate"),
    //    Message = "I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate, I'm a pirate"
    //},
    //new ChatMessageReceivedViewModel
    //{
    //    CommunityRole = new CommunityRole("magician"),
    //    Message = "I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician, I'm a magician"
    //},
    //new ChatMessageReceivedViewModel
    //{
    //    CommunityRole = new CommunityRole("tightropewalker"),
    //    Message = "I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor, I'm a survivor"
    //}
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
        var userMessage = UserMessage?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(userMessage))
        {
            return;
        }

        //var result = await _openAIService.SendTestCompletion().ConfigureAwait(true);
        var extraContext = string.Empty;
        if (_sendClipboardContentWithNextCompletion)
        {
            extraContext = _lastClipboardContent ?? string.Empty;
            _lastClipboardContent = string.Empty;
            _sendClipboardContentWithNextCompletion = false;
        }



        var result = await _openAIService.SendRealCompletion(_currentCommunityMember, userMessage, extraContext).ConfigureAwait(true);

        var message = $"{result.Content} [{result.MetaData.Usage.TotalTokens}]";

        MessageCollection.Add(new ChatMessageReceivedViewModel
        {
            IsActive = true,
            CommunityRole = result.CommunityRole,
            Message = message
        });

        UserMessage = string.Empty;
    }

    #endregion

    #region CommunityMemberSelectedCommand

    private RelayCommand? _communityMemberSelectedCommand;

    public ICommand CommunityMemberSelectedCommand => _communityMemberSelectedCommand ??= new RelayCommand(OnExecuteCommunityMemberSelectedCommand, OnCanExecuteCommunityMemberSelectedCommand);

    private bool OnCanExecuteCommunityMemberSelectedCommand()
    {
        return true;
    }

    private void LoadCommunityMembers()
    {
        CommunityMemberCollection = [
        new CommunityMemberViewModel
        {
            RoleDisplayName = @"Community_Member_Name_Pirate".GetLocalized(),
            RoleIcon = @"ms-appx:///Assets/Pirate.png",
            CommunityRole = new CommunityRole("pirate"),
            CharacterPrompts =
            [
                "You are One-Eyed Willy from 'The Goonies'. You are awkwardly charismatic. While you answer our questions you cannot help to express your hatred of Black Beard.  ",
                //"You hate Black Beard and you aren't quiet about it."
            ],
            Pithy = false,
            Temperature = 1.5
        },
        new CommunityMemberViewModel
        {
            RoleDisplayName = @"Community_Member_Name_Magician".GetLocalized(),
            RoleIcon = @"ms-appx:///Assets/Magician.png",
            CommunityRole = new CommunityRole("magician"),
            CharacterPrompts =
            [
                "You are a children's birthday party magician.  When you answer you like making words disappear and then reappear where we don't expect.  You like to be evasive in your answers, but in a playful way.  You enjoy using the magic wand emoji a lot.",
                //"You like making words disappear and than reappear.",
                //"You are a little too loose with the magic wand emoji."
            ],
            Pithy = true,
            Temperature = 1.0
        },
        new CommunityMemberViewModel
            {
                RoleDisplayName = @"Community_Member_Name_TightRopeWalker".GetLocalized(),
                RoleIcon = @"ms-appx:///Assets/TightropeWalker.png",
                CommunityRole = new CommunityRole("tightropewalker"),
                CharacterPrompts =
                [
                    "You are the black sheep of the flying Wallendas family. You aren't afraid of falling because you'll probably just respawn.  Whenever you write back to us you replace periods with underscores.",
                    //"You aren't afraid of falling because you'll probably just respawn",
                    //"You use _ where you should use ."
                ],
                Pithy = false,
                Temperature = 1.1
            },
            ];

        SelectedCommunityMember = CommunityMemberCollection.First();

        _currentCommunityMember = GetCommunityMember(SelectedCommunityMember);

        SetMessagePlaceHolder(_currentCommunityMember.RoleDisplayName);
    }

    private void OnExecuteCommunityMemberSelectedCommand()
    {
        if (SelectedCommunityMember is null)
        {
            _currentCommunityMember = GetDefaultCommunityMember();
        }

        _currentCommunityMember = GetCommunityMember(SelectedCommunityMember);

        SetMessagePlaceHolder(_currentCommunityMember.RoleDisplayName);
    }

    private static CommunityMember GetCommunityMember(CommunityMemberViewModel? subject)
    {
        if (subject is null)
        {
            return new CommunityMember();
        }

        return new CommunityMember
        {
            RoleDisplayName = subject?.RoleDisplayName ?? string.Empty,
            CommunityRole = subject?.CommunityRole ?? CommunityRole.Empty,
            CharacterPrompts = subject?.CharacterPrompts.ToList() ?? [],
            Temperature = subject?.Temperature ?? 1.0,
            Pithy = subject?.Pithy ?? true
        };
    }

    #endregion

    #region ToggleMonitoringClipboardCommand

    private RelayCommand? _toggleMonitoringClipboardCommand;

    public ICommand ToggleMonitoringClipboardCommand => _toggleMonitoringClipboardCommand ??= new RelayCommand(OnExecuteToggleMonitoringClipboardCommand, OnCanExecuteToggleMonitoringClipboardCommand);

    private bool OnCanExecuteToggleMonitoringClipboardCommand()
    {
        return true;
    }

    private void OnExecuteToggleMonitoringClipboardCommand()
    {
        if (IsMonitoringClipboard)
        {
            Clipboard.ContentChanged += OnClipboardContentChanged;

        }
        else
        {
            Clipboard.ContentChanged -= OnClipboardContentChanged;
        }
    }

    #endregion

    #endregion

    #endregion

    private CommunityMember GetDefaultCommunityMember()
    {
        return new CommunityMember
        {
            RoleDisplayName = "HeyGPT",
            CommunityRole = new CommunityRole("HeyGPT"),
            CharacterPrompts = [],
            Temperature = 1.0,
            Pithy = true
        };
    }

    private void SetMessagePlaceHolder(string roleDisplayName)
    {
        var formatString = "LetsChat_Enter_Message_TextBoxPlaceholder_Format_Text".GetLocalized();

        MessagePlaceholder = string.Format(formatString, roleDisplayName);
    }

    private async void OnClipboardContentChanged(object sender, object e)
    {
        DataPackageView dataPackageView = Clipboard.GetContent();
        if (dataPackageView.Contains(StandardDataFormats.Text))
        {
            _lastClipboardContent = await dataPackageView.GetTextAsync();
            _sendClipboardContentWithNextCompletion = true;
        }
    }

    }
