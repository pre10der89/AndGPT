using AndGPT.UI.Core.Contracts.Services;
using AndGPT.UI.Core.Events;
using Windows.ApplicationModel.DataTransfer;

namespace AndGPT.UI.Core.Services;

public class ClipboardService : IClipboardService
{
    public bool IsWatching { get; set; }

    public event EventHandler<ClipboardTextReceivedEventArgs>? TextReceived;
    public async Task<string> ReadTextAsync()
    {
        var dataPackageView = Clipboard.GetContent();

        if (!dataPackageView.Contains(StandardDataFormats.Text))
        {
            return string.Empty;
        }

        var text = await dataPackageView.GetTextAsync();

        return text ?? string.Empty;

    }

    public Task WriteTextAsync(string text)
    {
        throw new NotImplementedException();
    }

    public void EnableTextWatcher()
    {
        if (IsWatching)
        {
            return;
        }

        Clipboard.ContentChanged += OnClipboardContentChanged;

        IsWatching = true;
    }

    public void DisableTextWatcher()
    {
        Clipboard.ContentChanged += OnClipboardContentChanged;

        IsWatching = false;
    }

    private async void OnClipboardContentChanged(object? sender, object __)
    {
        var dataPackageView = Clipboard.GetContent();

        if (dataPackageView.Contains(StandardDataFormats.Text))
        {
            var text = await dataPackageView.GetTextAsync();

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            TextReceived?.Invoke(sender,new ClipboardTextReceivedEventArgs(text));
        }
    }
}
