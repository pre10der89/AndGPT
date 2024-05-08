using System.Collections.ObjectModel;

namespace HeyGPT.Core.Models;

public readonly record struct CommunityMember
{
    public CommunityMember()
    {
    }

    public string RoleDisplayName { get; init; } = string.Empty;

    public CommunityRole CommunityRole { get; init; } = CommunityRole.Empty;

    public List<string> CharacterPrompts { get; init; } = [];

    public double Temperature { get; init; } = 1.0;

    public bool Pithy { get; init; } = true;
}
