namespace AndGPT.Core.Events;

public readonly record struct ProtocolActivatedEvent
{
    public string Protocol { get; }

    public Uri Uri { get; }

    public ProtocolActivatedEvent(string protocol, Uri uri)
    {
        Protocol = protocol;
        Uri = uri;
    }
}
