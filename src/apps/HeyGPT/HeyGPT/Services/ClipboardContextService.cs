using AndGPT.Core.Contracts.Services;
using HeyGPT.App.Contracts.Policies;
using HeyGPT.App.Contracts.Services;
using AndGPT.Core.Models;

namespace HeyGPT.App.Services;

public class ClipboardContextService : IClipboardContextService, IDisposable
{
    #region Fields

    private readonly IClipboardContextPolicy _clipboardContextPolicy;
    private readonly IClipboardService _clipboardService;

    private IDisposable? _clipboardContentWatcherSubscription;

    #endregion

    #region Constructor(s)

    // ReSharper disable once ConvertToPrimaryConstructor
    public ClipboardContextService(IClipboardContextPolicy clipboardContextPolicy, IClipboardService clipboardService)
    {
        _clipboardContextPolicy = clipboardContextPolicy ?? throw new ArgumentNullException(nameof(clipboardContextPolicy));
        _clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));

        SubscribeToClipboardEvents();
    }

    #endregion

    #region Disposable Methods

    private bool _disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                UnsubscribeFromClipboardEvents();
            }

            _disposed = true;
        }
    }

    ~ClipboardContextService()
    {
        Dispose(false);
    }

    #endregion

    #region IClipboardContextService Members

    public async Task<ClipboardText> GetContextAsync()
    {
        var content = await _clipboardService.GetTextAsync();

        return _clipboardContextPolicy.ApplyPolicy(content);
    }

    #endregion

    #region Event Handlers

    private void SubscribeToClipboardEvents()
    {
        UnsubscribeFromClipboardEvents();

        _clipboardContentWatcherSubscription = _clipboardService.AddContentChangedWatcher(OnClipboardContentReceived);
    }


    public void UnsubscribeFromClipboardEvents()
    {
        _clipboardContentWatcherSubscription?.Dispose();
        _clipboardContentWatcherSubscription = null;
    }

    private void OnClipboardContentReceived(object? sender, EventArgs? e)
    {
        // TODO: Since this is a UI service we might consider using RxObservable streams instead of raising events up the chain.
    }

    #endregion
}
