namespace HeyGPT.Core.Models;

public readonly record struct ChatCompletionMetaData
{
    public ChatCompletionMetaData()
    {
        Id = string.Empty;
        Model = string.Empty;
        Created = DateTime.MinValue;
        Usage = ChatTokenUsage.Empty;
    }

    public ChatCompletionMetaData(string id, string? model, DateTimeOffset? created, ChatTokenUsage? tokenUsage)
    {
        Id = id;
        Model = model ?? string.Empty;
        Created = created ?? DateTime.MinValue;
        Usage = tokenUsage ?? ChatTokenUsage.Empty;
    }

    public string Id { get; }

    public string Model { get; }

    public DateTimeOffset? Created { get; }

    public ChatTokenUsage Usage { get; }
}
