namespace AndGPT.UI.Core.Events;

public class ClipboardTextReceivedEventArgs : EventArgs
{
    public string Text
    {
        get; init;
    } = string.Empty;

    public ClipboardTextReceivedEventArgs()
    {

    }

    public ClipboardTextReceivedEventArgs(string text)
    {
        Text = text;
    }
}
