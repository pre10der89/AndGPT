using Azure;
using Azure.AI.OpenAI;
using HeyGPT.Core.Models;

namespace HeyGPT.Core.Extensions;

internal static class ChatCompletionsOptionsExtensions
{
    public static void SetCompletionTemperature(this ChatCompletionsOptions? subject, float? value)
    {
        if (subject is null || !value.HasValue)
        {
            return;
        }

        subject.Temperature = value.Value;
    }

    public static void AddSystemMessage(this ChatCompletionsOptions? subject, string? message)
    {
        if (subject is null || string.IsNullOrEmpty(message))
        {
            return;
        }

        subject.Messages.Add(new ChatRequestSystemMessage(message));
    }

    public static void AddSystemMessages(this ChatCompletionsOptions? subject, IList<string>? messages)
    {
        if(subject is null || messages is null || !messages.Any())
        {
            return;
        }

        foreach (var message in messages)
        {
            subject.AddSystemMessage(message);
        }
    }

    public static void AddUserMessage(this ChatCompletionsOptions? subject, string? message)
    {
        if (subject is null || string.IsNullOrEmpty(message))
        {
            return;
        }

        subject.Messages.Add(new ChatRequestUserMessage(message));
    }


    public static void AddUserMessages(this ChatCompletionsOptions? subject, IList<string>? messages)
    {
        if (subject is null || messages is null || !messages.Any())
        {
            return;
        }

        foreach (var message in messages)
        {
            subject.AddUserMessage(message);
        }
    }
}
