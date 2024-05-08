using Azure.AI.OpenAI;

namespace HeyGPT.Core.Models;

public readonly record struct ChatTokenUsage
{
    public int CompletionTokens
    {
        get;
    }

    public int PromptTokens
    {
        get;
    }

    public int TotalTokens
    {
        get;
    }

    public static ChatTokenUsage Empty => new();

    public bool IsEmpty => CompletionTokens == 0 && PromptTokens == 0 && TotalTokens == 0;

    public ChatTokenUsage()
    {
        CompletionTokens = 0;
        PromptTokens = 0;
        TotalTokens = 0;
    }

    public ChatTokenUsage(int completionTokens, int promptTokens, int totalTokens)
    {
        CompletionTokens = completionTokens;
        PromptTokens = promptTokens;
        TotalTokens = totalTokens;
    }

    public static ChatTokenUsage operator +(ChatTokenUsage left, ChatTokenUsage right)
    {
        return new ChatTokenUsage(
            left.CompletionTokens + right.CompletionTokens,
            left.PromptTokens + right.PromptTokens,
            left.TotalTokens + right.TotalTokens
        );
    }

    public static ChatTokenUsage operator -(ChatTokenUsage left, ChatTokenUsage right)
    {
        return new ChatTokenUsage(
            left.CompletionTokens - right.CompletionTokens,
            left.PromptTokens - right.PromptTokens,
            left.TotalTokens - right.TotalTokens
        );
    }
}
