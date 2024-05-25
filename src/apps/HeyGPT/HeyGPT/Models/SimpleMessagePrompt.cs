#nullable disable

using System.Text.Json;

namespace HeyGPT.App.Models;

public readonly record struct SimpleMessagePrompt
{
    public SimpleMessagePrompt()
    {
    }

    public string Character { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;
}

public class SimpleMessagePromptCollection : List<SimpleMessagePrompt>
{
    public static SimpleMessagePromptCollection FromJson(string json)
    {
        return JsonSerializer.Deserialize<SimpleMessagePromptCollection>(json);
    }
}
