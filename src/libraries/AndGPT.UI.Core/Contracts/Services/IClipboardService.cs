using AndGPT.UI.Core.Events;

namespace AndGPT.UI.Core.Contracts.Services;

public interface IClipboardService
{
    bool IsWatching { get; }

    event EventHandler<ClipboardTextReceivedEventArgs> TextReceived;

    Task<string> ReadTextAsync();

    Task WriteTextAsync(string text);

    void EnableTextWatcher();

    void DisableTextWatcher();
}
