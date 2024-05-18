namespace AndGPT.Core.Models;

public sealed record ClipboardContent(ClipboardDataType DataType)
{
    public ClipboardText? Text
    {
        get; init;
    }
    public ClipboardImage? Image
    {
        get; init;
    }
    public Uri? Uri
    {
        get; init;
    }
    public ClipboardBinaryData? Binary
    {
        get; init;
    }

    public ClipboardContent(ClipboardText text) : this(ClipboardDataType.Text)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
    }

    public ClipboardContent(ClipboardImage image) : this(ClipboardDataType.Image)
    {
        Image = image ?? throw new ArgumentNullException(nameof(image));
    }

    public ClipboardContent(Uri uri) : this(ClipboardDataType.Image)
    {
        Uri = uri ?? throw new ArgumentNullException(nameof(uri));
    }

    public ClipboardContent(ClipboardBinaryData binaryData) : this(ClipboardDataType.Image)
    {
        Binary = binaryData ?? throw new ArgumentNullException(nameof(binaryData));
    }

    public static ClipboardContent Empty { get; } = new ClipboardContent(ClipboardDataType.None);

    public static ClipboardContent CreateTextContent(ClipboardText text)
    {
        return new ClipboardContent(text);
    }

    public static ClipboardContent CreateImageContent(ClipboardImage image)
    {
        return new ClipboardContent(image);
    }

    public static ClipboardContent CreateUriContent(Uri uri)
    {
        return new ClipboardContent(uri);
    }

    public static ClipboardContent CreateBinaryContent(ClipboardBinaryData binary)
    {
        return new ClipboardContent(binary);
    }

    public bool IsEmpty => DataType == ClipboardDataType.None;
}
