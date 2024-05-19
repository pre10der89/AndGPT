namespace AndGPT.Core.Models;

public sealed record ClipboardBinaryData
{
    public byte[] Data
    {
        get;
    }
    public string Format
    {
        get;
    }

    private ClipboardBinaryData(byte[] data, string format)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
        Format = format ?? throw new ArgumentNullException(nameof(format));
    }

    public static ClipboardBinaryData Create(byte[] data)
    {
        return Create(data, string.Empty);
    }

    public static ClipboardBinaryData Create(byte[] data, string format)
    {
        if (data == null || data.Length == 0)
        {
            throw new ArgumentException("Binary data cannot be null or empty.", nameof(data));
        }

        // Additional validation for supported formats can be added here
        return new ClipboardBinaryData(data, format);
    }

    public static ClipboardBinaryData Empty { get; } = Create([], string.Empty);

    public bool IsEmpty => Data.Length == 0;

    public override string ToString() => $"Format: {Format}, Size: {Data.Length} bytes";
}
