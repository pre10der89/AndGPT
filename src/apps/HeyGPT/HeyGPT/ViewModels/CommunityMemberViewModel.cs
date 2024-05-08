using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HeyGPT.Core.Models;

namespace HeyGPT.App.ViewModels;

public partial class CommunityMemberViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string _roleDisplayName = string.Empty; 

    [ObservableProperty]
    private string _roleIcon = string.Empty;

    [ObservableProperty]
    private CommunityRole _communityRole = CommunityRole.Empty;

    public ObservableCollection<string> CharacterPrompts { get; init; } = [];

    [ObservableProperty]
    private double _temperature = 1.0;

    [ObservableProperty]
    private bool _pithy = true;
}
