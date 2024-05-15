namespace HeyGPT.App.Contracts.Services;

public interface IClipboardContextService2
{
    bool IsEnabled { get; set; }

    bool HasContext { get; }

    string GetContext();

    void ClearContext();
}
