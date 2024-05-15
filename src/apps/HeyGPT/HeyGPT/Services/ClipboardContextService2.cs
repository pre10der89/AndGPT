using AndGPT.UI.Core.Contracts.Services;
using HeyGPT.App.Contracts.Services;

namespace HeyGPT.App.Services;

public class ClipboardContextService2 : IClipboardContextService2
{
    private readonly IClipboardService _clipboardService;
    private bool _isClipboardContextEnabled;
    private string _lastClipboardContent = string.Empty;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ClipboardContextService2(IClipboardService clipboardService)
    {
        _clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));
    }

    public bool IsEnabled
    {
        get => _isClipboardContextEnabled;
        set
        {
            if (value)
            {
                SubscribeToEvents();
            }
            else
            {
                UnsubscribeFromEvents();
            }

            _isClipboardContextEnabled = value;
        }
    }

    public bool HasContext
    {
        get;
        set;
    }

    public string GetContext()
    {
        var content = _lastClipboardContent;

        ClearContext();

        return content;
    }

    public void ClearContext()
    {
        _lastClipboardContent = string.Empty;
        HasContext = false;
    }

    private void SubscribeToEvents()
    {
        _clipboardService.TextReceived += OnClipboardTextReceived;

        _isClipboardContextEnabled = true;
    }


    public void UnsubscribeFromEvents()
    {
        _clipboardService.TextReceived -= OnClipboardTextReceived;

        _isClipboardContextEnabled = false;

        ClearContext();
    }

    private void OnClipboardTextReceived(object? sender, AndGPT.UI.Core.Events.ClipboardTextReceivedEventArgs? e)
    {
        if (e is null || string.IsNullOrEmpty(e.Text))
        {
            return;
        }

        _lastClipboardContent = e.Text;
        HasContext = true;
    }
}
