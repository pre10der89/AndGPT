namespace HeyGPT.Core.Models;

public readonly record struct ChatCharacterDetails
{
    public ChatCharacterDetails()
    {
    }

    public string RoleDisplayName { get; init; } = string.Empty;

    public CharacterType CharacterType { get; init; } = CharacterType.Default;

    public List<string> AdditionalCharacteristics { get; init; } = [];

    public float? Temperature { get; init; } = null;

    public bool ShouldBePithy { get; init; } = true;
}
