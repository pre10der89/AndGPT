using AndGPT.Core.Models;

namespace AndGPT.Core.Contracts.Services;

// TODO: We can probably just use an Action for the content changed event since we aren't allowing access to the C# event handler.
// TODO: Need to determine if exposing a well-groomed observable infrastructure here like RxObservables is worth it.  Once we introduce such a coupling it'll go everywhere :):)
// TODO: There are many other types of content on windows like HTML, RTF, etc. that could be returned.  If this was made into a platform-agnostic service then we'd want to compare notes.
         
//     

/// <summary>
/// Describes the interface for interacting with the operating system clipboard.
/// </summary>
public interface IClipboardService
{
    /// <summary>
    /// Adds a subscription to the clipboard content changed event.
    /// </summary>
    /// <param name="handler">The event handler to be invoked.</param>
    /// <returns>A disposable subscription reference.</returns>
    IDisposable AddContentChangedWatcher(EventHandler<EventArgs> handler);

    /// <summary>
    /// Removes the subscription to the clipboard content changed event.
    /// </summary>
    /// <param name="handler"></param>
    void RemoveContentChangedWatcher(EventHandler<EventArgs> handler);

    /// <summary>
    /// Gets the text content from the clipboard.
    /// </summary>
    /// <returns>A string representing the clipboard's text content.  Otherwise, the empty string will be returned.</returns>
    Task<ClipboardText> GetTextAsync();

    /// <summary>
    /// Sets the specified text onto the clipboard.
    /// </summary>
    /// <param name="text">The <see cref="ClipboardText"/> to be set on the clipboard.</param>
    /// <returns>True, if the set operation was successful. Otherwise, false.</returns>
    Task<bool> SetTextAsync(ClipboardText text);

    /// <summary>
    /// Sets the specified text onto the clipboard with the specified <see cref="ClipboardContentSettings"/>
    /// </summary>
    /// <param name="text">The non-empty <see cref="ClipboardText"/> to be set on the clipboard.</param>
    /// <param name="settings">The clipboard content options to associate with this operation.</param>
    /// <returns>True, if the set operation was successful. Otherwise, false.</returns>
    Task<bool> SetTextAsync(ClipboardText text, ClipboardContentSettings settings);

    /// <summary>
    /// Gets the Uri content from the clipboard.
    /// </summary>
    /// <returns>If Uri content was found the method will return a well-formed Uri.  Otherwise, null will be returned.</returns>
    Task<Uri?> GetUriAsync();

    /// <summary>
    /// Sets the specified URI onto the clipboard.
    /// </summary>
    /// <param name="uri">The uri to set on the clipboard.</param>
    /// <returns>True, if the set operation was successful. Otherwise, false.</returns>
    Task<bool> SetUriAsync(Uri uri);

    /// <summary>
    /// Sets the specified text onto the clipboard with the specified <see cref="ClipboardContentSettings"/>
    /// </summary>
    /// <param name="uri">The uri to set on the clipboard.</param>
    /// <param name="settings">The clipboard content options to associate with this operation.</param>
    /// <returns>True, if the set operation was successful. Otherwise, false.</returns>
    Task<bool> SetUriAsync(Uri uri, ClipboardContentSettings settings);

    /// <summary>
    /// Gets the image (bitmap) content from the clipboard.
    /// </summary>
    /// <returns>
    /// If image (bitmap) data is found then a non-empty <see cref="ClipboardImage"/> will be returned.
    /// Otherwise, if no image data is found an empty <see cref="ClipboardImage"/> will be returned.
    /// </returns>
    Task<ClipboardImage> GetImageAsync();

    /// <summary>
    /// Sets the specified image onto the clipboard.
    /// </summary>
    /// <param name="image">The image to set on the clipboard.</param>
    /// <returns>True, if the set operation was successful. Otherwise, false.</returns>
    Task<bool> SetImageAsync(ClipboardImage image);

    /// <summary>
    /// Sets the specified image onto the clipboard with the specified <see cref="ClipboardContentSettings"/>
    /// </summary>
    /// <param name="image">The image to set on the clipboard.</param>
    /// <param name="settings">The clipboard content options to associate with this operation.</param>
    /// <returns>True, if the set operation was successful. Otherwise, false.</returns>
    Task<bool> SetImageAsync(ClipboardImage image, ClipboardContentSettings settings);

    /// <summary>
    /// Gets binary content from the clipboard.
    /// </summary>
    /// <returns>
    /// If binary data is found then a non-empty <see cref="ClipboardBinaryData"/> will be returned.
    /// Otherwise, if no binary data is found an empty <see cref="ClipboardBinaryData"/> will be returned.
    /// </returns>
    Task<ClipboardBinaryData> GetBinaryAsync(ClipboardFormat format);

    /// <summary>
    /// Sets the specified binary data onto the clipboard.
    /// </summary>
    /// <param name="data">The binary data to set on the clipboard.</param>
    /// <returns>True, if the set operation was successful. Otherwise, false.</returns>
    Task<bool> SetBinaryAsync(ClipboardBinaryData data);

    /// <summary>
    /// Sets the specified binary data onto the clipboard.
    /// </summary>
    /// <param name="data">The binary data to set on the clipboard.</param>
    /// <param name="settings">The clipboard content options to associate with this operation.</param>
    /// <returns>True, if the set operation was successful. Otherwise, false.</returns>
    Task<bool> SetBinaryAsync(ClipboardBinaryData data, ClipboardContentSettings settings);

    /// <summary>
    /// Gets the content of the clipboard.
    /// </summary>
    /// <returns>
    /// If content is found then a non-empty <see cref="ClipboardContent"/> is returned.
    /// Otherwise, an empty <see cref="ClipboardContent"/> will be returned.
    /// </returns>
    Task<ClipboardContent> GetContentAsync();

    /// <summary>
    /// Sets the specified content onto the clipboard.
    /// </summary>
    /// <param name="content">The content to set on the clipboard.</param>
    /// <returns>True, if the set operation was successful. Otherwise, false.</returns>
    Task<bool> SetContentAsync(ClipboardContent content);

    /// <summary>
    /// Sets the specified content onto the clipboard.
    /// </summary>
    /// <param name="content">The content to set on the clipboard.</param>
    /// <param name="settings">The clipboard content options to associate with this operation.</param>
    /// <returns>True, if the set operation was successful. Otherwise, false.</returns>
    Task<bool> SetContentAsync(ClipboardContent content, ClipboardContentSettings settings);
}
