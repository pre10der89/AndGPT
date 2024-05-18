namespace AndGPT.Core.Models;

public readonly record struct ClipboardContentSettings()
{
    /// <summary>
    /// Gets or initializes a value indicating whether the new clipboard data can be saved to the clipboard history.
    /// </summary>
    public bool IsAllowedInHistory { get; init; } = true;

    /// <summary>
    /// Gets or initializes a value indicating whether the new clipboard data can be synced to other devices.
    /// </summary>
    public bool IsRoamable { get; init; } = true;

    public static ClipboardContentSettings Default { get; } = new();
}
