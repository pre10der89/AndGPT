using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HeyGPT.Core.Models;

namespace HeyGPT.App.ViewModels;

public partial class ChatCharacterViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string _roleDisplayName = string.Empty; 

    [ObservableProperty]
    private string _roleIcon = string.Empty;

    [ObservableProperty]
    private CharacterType _characterType = CharacterType.Default;

    public ObservableCollection<string> CharacterPrompts { get; init; } = [];

    [ObservableProperty] private float? _temperature = null;

    [ObservableProperty]
    private bool _pithy = true;
}
