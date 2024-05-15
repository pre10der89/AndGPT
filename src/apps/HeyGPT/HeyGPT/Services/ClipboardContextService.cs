using AndGPT.UI.Core.Contracts.Services;
using HeyGPT.App.Contracts.Services;

namespace HeyGPT.App.Services;

public class ClipboardContextService : IClipboardContextService
{
    private readonly IClipboardService _clipboardService;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ClipboardContextService(IClipboardService clipboardService)
    {
        _clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));
    }

    public async Task<string> GetContextAsync()
    {
        var content = await _clipboardService.ReadTextAsync();

        var limitedContext = content.Length > 300 ? content.Substring(0, 300) : content;

        return limitedContext;
    }
}
