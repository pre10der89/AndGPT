using Azure.AI.OpenAI;

namespace HeyGPT.Core.Models;

public readonly record struct ChatCompletionContent
{
    public ChatRole ChatRole
    {
        get; init;
    }

    public string Message
    {
        get;
    }

    public static ChatCompletionContent Empty => new();

    public bool IsEmpty => string.IsNullOrEmpty(Message);

    public ChatCompletionContent()
    {
        Message = string.Empty;
    }

    public ChatCompletionContent(ChatRole chatRole, string message)
    {
        ChatRole = chatRole;
        Message = message ?? throw new ArgumentNullException(nameof(message), "The message should not be null");
    }


    /// <summary>
    /// Extract the string representation of the specified <see cref="ChatCompletionContent"/>.
    /// </summary>
    /// <param name="value">The <see cref="ChatCompletionContent"/> object from which the string representation will be extracted.</param>
    public static implicit operator string(ChatCompletionContent value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value.Message;
    }

    public override string ToString() => Message;
}
