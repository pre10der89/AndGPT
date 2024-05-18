namespace AndGPT.Core.Models;

public sealed record ClipboardImage
{
    public byte[] Data
    {
        get;
    }

    public string Format
    {
        get;
    }

    private ClipboardImage(byte[] data, string format)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));

        Format = format ?? throw new ArgumentNullException(nameof(format));
    }

    public static ClipboardImage Create(byte[] data, string format)
    {
        if (data == null || data.Length == 0)
        {
            throw new ArgumentException("Image data cannot be null or empty.", nameof(data));
        }

        // Additional validation for supported formats can be added here
        return new ClipboardImage(data, format);
    }

    public static ClipboardImage Empty { get; } = Create([], string.Empty);

    public bool IsEmpty => Data.Length == 0;

    public override string ToString() => $"Format: {Format}, Size: {Data.Length} bytes";
}
