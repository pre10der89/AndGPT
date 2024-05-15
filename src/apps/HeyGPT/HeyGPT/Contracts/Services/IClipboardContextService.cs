namespace HeyGPT.App.Contracts.Services;

public interface IClipboardContextService
{
    Task<string> GetContextAsync();
}
