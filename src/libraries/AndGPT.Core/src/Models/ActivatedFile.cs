namespace AndGPT.Core.Models;

public readonly record struct ActivatedFile
{
    public string Name { get; init; }

    public string Path { get; init; }

    public FileAttributes Attributes { get; init; }

    public DateTimeOffset DateCreated { get; init; }

    public ActivatedFile(string name, string path, FileAttributes attributes, DateTimeOffset dateCreated)
    {
        Name = name;
        Path = path;
        Attributes = attributes;
        DateCreated = dateCreated;
    }
}
