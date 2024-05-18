using AndGPT.Core.Contracts.Services;
using AndGPT.Core.Helpers;
using AndGPT.Core.Models;
using AndGPT.UI.Core.Helpers;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace AndGPT.UI.Core.Services;

public class WindowsClipboardService : IClipboardService
{
    #region Fields

    private int _subscriptionCount;
    private readonly object _lock = new();
    private readonly List<WeakEventHandler<EventArgs>> _contentChangedSubjects = new();

    private readonly Dictionary<string, string> _formatMappings = new()
    {
        { ClipboardFormat.Text.ToString(), StandardDataFormats.Text },
        { ClipboardFormat.Bitmap.ToString(), StandardDataFormats.Bitmap },
        { ClipboardFormat.Uri.ToString(), StandardDataFormats.Uri }
    };

    #endregion

    #region Constructor(s)

    // ReSharper disable once EmptyConstructor (for now)
    public WindowsClipboardService()
    {
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
                // RemoveContentChangedWatcher all event handlers
                lock (_lock)
                {
                    Clipboard.ContentChanged -= OnClipboardContentChanged;
                    _contentChangedSubjects.Clear();
                    _subscriptionCount = 0;
                }
            }
            _disposed = true;
        }
    }

    ~WindowsClipboardService()
    {
        Dispose(false);
    }

    #endregion

    #region IClipboardService Members

    #region Events

    // NOTE: Using a C# event is guaranteed to work in all OS platforms.
    //      There are possible better ways for us to stream that content has changed... We could use RxObservables here instead to provide the consumer with a richer experience.
    //      This is a low-level service and introducing RxObservables here might involve transitively coupling that library all over the place.  We might consider a higher level service
    //      that wraps 

    public event EventHandler<EventArgs> ContentChanged
    {
        add
        {
            lock (_lock)
            {
                if (_subscriptionCount == 0)
                {
                    Clipboard.ContentChanged += OnClipboardContentChanged;
                }

                _subscriptionCount++;
                var weakHandler = new WeakEventHandler<EventArgs>(value);
                _contentChangedSubjects.Add(weakHandler);
            }
        }
        remove
        {
            lock (_lock)
            {
                var weakHandler = _contentChangedSubjects.FirstOrDefault(w => w.HandlerProxy == value);

                if (weakHandler == null || !_contentChangedSubjects.Remove(weakHandler))
                {
                    return;
                }

                _subscriptionCount--;
                if (_subscriptionCount == 0)
                {
                    Clipboard.ContentChanged -= OnClipboardContentChanged;
                }
            }
        }
    }

    #endregion

    #region Methods

    public IDisposable AddContentChangedWatcher(EventHandler<EventArgs> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        ContentChanged -= handler;
        ContentChanged += handler;

        return new DisposableSubscription(this, handler);
    }

    public void RemoveContentChangedWatcher(EventHandler<EventArgs> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        ContentChanged -= handler;
    }

    public ClipboardDataType GetClipboardDataType()
    {
        // NOTE: We are using an enumeration here to try to be as generic as possible.  We have the ClipboardFormat that we use in various places, and maybe we should be consistent, however, we
        //       are not sure if any custom string format types can be recovered.
        // TODO: Do more research to see if we can retrieve custom format types from the clipboard history.  

        var dataPackageView = GetContentFromClipboard();

        if (dataPackageView is null)
        {
            return ClipboardDataType.None;
        }

        if (dataPackageView.Contains(StandardDataFormats.Text))
        {
            return ClipboardDataType.Text;
        }
        if (dataPackageView.Contains(StandardDataFormats.Bitmap))
        {
            return ClipboardDataType.Image;
        }
        if (dataPackageView.Contains(StandardDataFormats.Uri))
        {
            return ClipboardDataType.Uri;
        }

        // Check for other binary formats or custom formats
        foreach (var format in _formatMappings.Values)
        {
            if (dataPackageView.Contains(format))
            {
                return ClipboardDataType.Binary;
            }
        }

        // If no known formats match, return Custom
        return ClipboardDataType.Custom;
    }

    public async Task<ClipboardText> GetTextAsync()
    {
        // TODO: We need an enumeration of text types similar to the ClipboardFormat and we need a platform to service domain transformer.

        var dataPackageView = GetContentFromClipboard();

        if (dataPackageView is not null && dataPackageView.Contains(StandardDataFormats.Text))
        {
            var value = await dataPackageView.GetTextAsync();

            if (value is not null)
            {
                return ClipboardText.Create(value, StandardDataFormats.Text);
            }
        }

        if (dataPackageView is not null && dataPackageView.Contains(StandardDataFormats.Html))
        {
            var value = await dataPackageView.GetHtmlFormatAsync();

            if (value is not null)
            {
                return ClipboardText.Create(value, StandardDataFormats.Html);
            }
        }

        if (dataPackageView is not null && dataPackageView.Contains(StandardDataFormats.Rtf))
        {
            var value = await dataPackageView.GetRtfAsync();

            if (value is not null)
            {
                return ClipboardText.Create(value, StandardDataFormats.Rtf);
            }
        }

        return ClipboardText.Empty;
    }

    public async Task<bool> SetTextAsync(ClipboardText text)
    {
        return await SetTextAsync(text, ClipboardContentSettings.Default);
    }

    public async Task<bool> SetTextAsync(ClipboardText text, ClipboardContentSettings settings)
    {
        // TODO: Set RTF and HTML type texts?

        if (text.IsEmpty)
        {
            return false;
        }

        var dataPackage = new DataPackage();

        dataPackage.SetText(text.Text);

        await Task.CompletedTask;

        return SetContentOnClipboard(dataPackage, settings);
    }

    public async Task<Uri?> GetUriAsync()
    {
        var dataPackageView = GetContentFromClipboard();

        if (dataPackageView is not null && dataPackageView.Contains(StandardDataFormats.Uri))
        {
            return await dataPackageView.GetUriAsync();
        }

        return null;
    }

    public async Task<bool> SetUriAsync(Uri uri)
    {
        return await SetUriAsync(uri, ClipboardContentSettings.Default);
    }

    public async Task<bool> SetUriAsync(Uri uri, ClipboardContentSettings settings)
    {
        ArgumentNullException.ThrowIfNull(uri);

        var dataPackage = new DataPackage();

        dataPackage.SetUri(uri);

        await Task.CompletedTask;

        return SetContentOnClipboard(dataPackage, settings);
    }

    public async Task<ClipboardImage> GetImageAsync()
    {
        var dataPackageView = GetContentFromClipboard();

        if (dataPackageView is null || !dataPackageView.Contains(StandardDataFormats.Bitmap))
        {
            return ClipboardImage.Empty;
        }

        var reference = await dataPackageView.GetBitmapAsync();

        if (reference == null)
        {
            return ClipboardImage.Empty;
        }

        var stream = await reference.OpenReadAsync();

        var data = await ClipboardHelper.ReadStreamAsync(stream);

        return ClipboardImage.Create(data, "Bitmap");
    }

    public async Task<bool> SetImageAsync(ClipboardImage image)
    {
        return await SetImageAsync(image, ClipboardContentSettings.Default);
    }

    public async Task<bool> SetImageAsync(ClipboardImage image, ClipboardContentSettings settings)
    {
        ArgumentNullException.ThrowIfNull(image);

        var dataPackage = new DataPackage();
        var stream = await ClipboardHelper.WriteStreamAsync(image.Data);

        dataPackage.SetBitmap(RandomAccessStreamReference.CreateFromStream(stream));
        
        return SetContentOnClipboard(dataPackage, settings);
    }

    public async Task<ClipboardBinaryData> GetBinaryAsync(ClipboardFormat format)
    {
        var platformFormat = GetPlatformSpecificFormat(format);
        var dataPackageView = GetContentFromClipboard();

        if (dataPackageView is null || !dataPackageView.Contains(platformFormat))
        {
            return ClipboardBinaryData.Empty;
        }

        if (await dataPackageView.GetDataAsync(platformFormat) is not IRandomAccessStream stream)
        {
            return ClipboardBinaryData.Empty;
        }

        var data = await ClipboardHelper.ReadStreamAsync(stream);

        return ClipboardBinaryData.Create(data, format.ToString());
    }

    public async Task<bool> SetBinaryAsync(ClipboardBinaryData data)
    {
        return await SetBinaryAsync(data, ClipboardContentSettings.Default);
    }

    public async Task<bool> SetBinaryAsync(ClipboardBinaryData data, ClipboardContentSettings settings)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        if (data.Data.Length == 0)
        {
            return false;
        }

        var platformFormat = GetPlatformSpecificFormat(ClipboardFormat.Custom(data.Format));
        var dataPackage = new DataPackage();
        var stream = await ClipboardHelper.WriteStreamAsync(data.Data);

        dataPackage.SetData(platformFormat, RandomAccessStreamReference.CreateFromStream(stream));

        return SetContentOnClipboard(dataPackage, settings);
    }

    public async Task<ClipboardContent> GetContentAsync()
    {
        var dataPackageView = GetContentFromClipboard();

        if (dataPackageView is null)
        {
            return ClipboardContent.Empty;
        }

        if (dataPackageView.Contains(StandardDataFormats.Text))
        {
            var value = await GetTextAsync().ConfigureAwait(false);

            return ClipboardContent.CreateTextContent(value);
        }

        if (dataPackageView.Contains(StandardDataFormats.Uri))
        {
            var value = await GetUriAsync().ConfigureAwait(false);

            return value is not null ? ClipboardContent.CreateUriContent(value) : ClipboardContent.Empty;
        }

        if (dataPackageView.Contains(StandardDataFormats.Bitmap))
        {
            var value = await GetImageAsync().ConfigureAwait(false);

            return ClipboardContent.CreateImageContent(value);
        }

        return ClipboardContent.Empty;
    }

    public async Task<bool> SetContentAsync(ClipboardContent content)
    {
        return await SetContentAsync(content, ClipboardContentSettings.Default);
    }

    public async Task<bool> SetContentAsync(ClipboardContent content, ClipboardContentSettings settings)
    {
        ArgumentNullException.ThrowIfNull(content);

        switch (content.DataType)
        {
            case ClipboardDataType.Text:
                return await SetTextAsync(content.Text ?? ClipboardText.Empty, settings).ConfigureAwait(false);
            case ClipboardDataType.Image:
                return await SetImageAsync(content.Image ?? ClipboardImage.Empty, settings).ConfigureAwait(false);
            case ClipboardDataType.Uri:
                return content.Uri != null && await SetUriAsync(content.Uri, settings).ConfigureAwait(false);
            case ClipboardDataType.Binary:
                await SetBinaryAsync(content.Binary ?? ClipboardBinaryData.Empty, settings).ConfigureAwait(false);
                break;
            case ClipboardDataType.None:
            case ClipboardDataType.Custom:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    #endregion

    #endregion

    #region Event Handlers

    private void OnClipboardContentChanged(object? sender, object? e)
    {
        List<WeakEventHandler<EventArgs>> handlers;

        lock (_lock)
        {
            handlers = _contentChangedSubjects.ToList();
        }

        foreach (var handler in handlers)
        {
            _ = Task.Run(() =>
            {
                try
                {
                    handler.HandlerProxy.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    // Log or handle the exception as appropriate
                    Console.Error.WriteLine($"Error in clipboard content changed handler: {ex}");
                }
            });
        }
    }

    #endregion

    #region Private Methods

    private string GetPlatformSpecificFormat(ClipboardFormat format)
    {
        return _formatMappings.TryGetValue(format.ToString(), out var platformFormat) ? platformFormat : format.ToString();
    }

    private class DisposableSubscription(IClipboardService clipboardService, EventHandler<EventArgs> handler) : IDisposable
    {
        public void Dispose()
        {
            clipboardService.RemoveContentChangedWatcher(handler);
        }
    }

    private static DataPackageView? GetContentFromClipboard()
    {
        return Clipboard.GetContent();
    }

    private static bool SetContentOnClipboard(DataPackage? dataPackage, ClipboardContentSettings settings)
    {
        // Setting clipboard data will only work if the application is in the foreground, or when the debugger is attached.
        // Clipboard.SetContentOnClipboard will throw an exception if the application is not in the foreground.
        // Clipboard.SetContentWithOptions will return false if there is an issue setting content including because of the application's foreground status.

        return Clipboard.SetContentWithOptions(dataPackage, new ClipboardContentOptions
        {
            IsAllowedInHistory = settings.IsAllowedInHistory,
            IsRoamable = settings.IsRoamable
        });
    }

    #endregion
}
