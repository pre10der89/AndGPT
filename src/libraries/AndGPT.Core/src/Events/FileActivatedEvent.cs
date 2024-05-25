using AndGPT.Core.Models;

namespace AndGPT.Core.Events;

public readonly record struct FileActivatedEvent
{
    public string Verb { get; }

    public IReadOnlyList<ActivatedFile> Files { get; }

    public FileActivatedEvent(string verb, IList<ActivatedFile> files)
    {
        Verb = verb;
        Files = files.AsReadOnly();
    }
}
