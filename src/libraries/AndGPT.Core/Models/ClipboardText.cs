namespace AndGPT.Core.Models;

public sealed record ClipboardText
{
    public string Text
    {
        get;
    }

    public string Format
    {
        get;
    }

    private ClipboardText(string text, string format)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));

        Format = format ?? throw new ArgumentNullException(nameof(format));
    }

    public static ClipboardText Create(string text, string format)
    {
        ArgumentNullException.ThrowIfNull(text);

        return new ClipboardText(text, format ?? string.Empty);
    }

    public static ClipboardText Empty { get; } = Create(string.Empty, string.Empty);

    public bool IsEmpty => Text.Length == 0;

    public override string ToString() => $"Format: {Format}, Length: {Text.Length} bytes";
}
