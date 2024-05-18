using AndGPT.Core.Models;

namespace HeyGPT.App.Contracts.Services;

public interface IClipboardContextService
{
    Task<ClipboardText> GetContextAsync();
}
