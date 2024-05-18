using AndGPT.Core.Models;

namespace HeyGPT.App.Contracts.Policies;

public interface IClipboardContextPolicy
{
    ClipboardText ApplyPolicy(ClipboardText text);
}
