using Windows.Storage.Streams;

namespace AndGPT.UI.Core.Helpers;

public static class ClipboardHelper
{
    public static async Task<byte[]> ReadStreamAsync(IRandomAccessStream stream)
    {
        using var reader = new BinaryReader(stream.AsStreamForRead());
        return await Task.FromResult(reader.ReadBytes((int)stream.Size));
    }

    public static async Task<byte[]> ReadStreamAsync(IRandomAccessStreamReference streamReference)
    {
        var stream = await streamReference.OpenReadAsync();
        using var reader = new DataReader(stream);
        var bytes = new byte[stream.Size];
        await reader.LoadAsync((uint)stream.Size);
        reader.ReadBytes(bytes);
        return bytes;
    }

    public static async Task<IRandomAccessStream> WriteStreamAsync(byte[] data)
    {
        var stream = new InMemoryRandomAccessStream();
        var writer = new DataWriter(stream);

        writer.WriteBytes(data);
        await writer.StoreAsync();
        writer.DetachStream(); // Detach the stream so that it is not disposed
        writer.Dispose();

        stream.Seek(0);
        return stream;
    }
}
