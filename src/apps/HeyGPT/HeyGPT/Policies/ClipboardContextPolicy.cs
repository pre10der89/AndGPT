using AndGPT.Core.Models;
using HeyGPT.App.Contracts.Policies;

namespace HeyGPT.App.Policies;

public class ClipboardContextPolicy : IClipboardContextPolicy
{
    // TODO: We can inject an options service here to get data about constraints, etc. 

    private const int MaximumClipboardSize = 512;

    public ClipboardText ApplyPolicy(ClipboardText text)
    {
        if (text.IsEmpty)
        {
            return ClipboardText.Empty;
        }

        return ClipboardText.Create(text.Text.Length > MaximumClipboardSize ? text.Text[..MaximumClipboardSize] : text.Text, text.Format);
    }
}