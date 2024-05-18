using Windows.Storage.Streams;

namespace AndGPT.UI.Core.Helpers;

public static class ClipboardHelper
{
    public static async Task<byte[]> ReadStreamAsync(IRandomAccessStream stream)
    {
        using var reader = new BinaryReader(stream.AsStreamForRead());
        return await Task.FromResult(reader.ReadBytes((int)stream.Size));
    }

    public static async Task<IRandomAccessStream> WriteStreamAsync(byte[] data)
    {
        var stream = new InMemoryRandomAccessStream();
        using (var writer = new DataWriter(stream))
        {
            writer.WriteBytes(data);
            await writer.StoreAsync();
        }
        stream.Seek(0);
        return stream;
    }
}
