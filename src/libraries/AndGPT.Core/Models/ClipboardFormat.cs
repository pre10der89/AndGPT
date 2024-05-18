namespace AndGPT.Core.Models;

public readonly record struct ClipboardFormat
{
    public string Format
    {
        get;
    }

    private ClipboardFormat(string format)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(format, nameof(format));

        Format = format;
    }

    public static ClipboardFormat Text { get; } = new("Text");
    public static ClipboardFormat Bitmap { get; } = new("Bitmap");
    public static ClipboardFormat Uri { get; } = new("Uri");

    public static ClipboardFormat Custom(string format)
    {
        return new ClipboardFormat(format);
    }

    public override string ToString() => Format;
}

