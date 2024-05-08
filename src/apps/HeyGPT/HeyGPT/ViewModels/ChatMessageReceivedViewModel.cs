using CommunityToolkit.Mvvm.ComponentModel;
using HeyGPT.Core.Models;

namespace HeyGPT.App.ViewModels;

public partial class ChatMessageReceivedViewModel : ObservableRecipient
{
    [ObservableProperty]
    private CommunityRole? _communityRole;

    [ObservableProperty]
    private string _message;
}
