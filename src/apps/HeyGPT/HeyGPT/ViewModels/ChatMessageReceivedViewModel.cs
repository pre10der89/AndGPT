using CommunityToolkit.Mvvm.ComponentModel;
using HeyGPT.Core.Models;

namespace HeyGPT.App.ViewModels;

public partial class ChatMessageReceivedViewModel : ObservableRecipient
{
    [ObservableProperty]
    private CharacterType? _communityRole;

    [ObservableProperty]
    private string _message = string.Empty;
}
