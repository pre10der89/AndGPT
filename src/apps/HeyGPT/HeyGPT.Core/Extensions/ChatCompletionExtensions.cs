using Azure;
using Azure.AI.OpenAI;
using HeyGPT.Core.Models;

namespace HeyGPT.Core.Extensions;

internal static class ChatCompletionExtensions
{
    public static ChatTokenUsage GetTokenUsage(this Response<ChatCompletions>? subject)
    {
        if (subject is null || !subject.HasValue)
        {
            return ChatTokenUsage.Empty;
        }

        return GetTokenUsage(subject.Value);
    }

    public static ChatTokenUsage GetTokenUsage(this ChatCompletions? chatCompletions)
    {
        if(chatCompletions?.Usage is null)
        {
            return ChatTokenUsage.Empty;
        }

        return new ChatTokenUsage(chatCompletions.Usage.CompletionTokens, chatCompletions.Usage.PromptTokens, chatCompletions.Usage.TotalTokens);
    }

    public static string GetCompletionId(this Response<ChatCompletions>? subject)
    {
        if (subject is null || !subject.HasValue)
        {
            return string.Empty;
        }

        return GetCompletionId(subject.Value);
    }

    public static string GetCompletionId(this ChatCompletions? chatCompletions)
    {
        return string.IsNullOrWhiteSpace(chatCompletions?.Id) ? string.Empty : chatCompletions.Id;
    }

    public static string GetModel(this Response<ChatCompletions>? subject)
    {
        if (subject is null || !subject.HasValue)
        {
            return string.Empty;
        }

        return GetModel(subject.Value);
    }

    public static string GetModel(this ChatCompletions? chatCompletions)
    {
        return string.IsNullOrWhiteSpace(chatCompletions?.Model) ? string.Empty : chatCompletions.Model;
    }

    public static DateTimeOffset GetCreationDate(this Response<ChatCompletions>? subject)
    {
        if (subject is null || !subject.HasValue)
        {
            return DateTimeOffset.MinValue;
        }

        return GetCreationDate(subject.Value);
    }

    public static DateTimeOffset GetCreationDate(this ChatCompletions? chatCompletions)
    {
        return chatCompletions?.Created ?? DateTimeOffset.MinValue;
    }

    public static ChatCompletionMetaData GetMetaData(this Response<ChatCompletions>? subject)
    {
        if (subject is null || !subject.HasValue)
        {
            return new ChatCompletionMetaData();
        }

        return GetMetaData(subject.Value);
    }

    public static ChatCompletionMetaData GetMetaData(this ChatCompletions? chatCompletions)
    {
        return chatCompletions is null ? new ChatCompletionMetaData() : new ChatCompletionMetaData(chatCompletions.GetCompletionId(), chatCompletions.GetModel(), chatCompletions.GetCreationDate(), chatCompletions.GetTokenUsage());
    }

    public static IReadOnlyList<ChatChoice> GetChatChoicesList(this ChatCompletions? subject)
    {
        if (subject?.Choices is null || subject.Choices.Count == 0)
        {
            return [];
        }

        return subject.Choices;
    }

    public static ChatChoice? GetFirstChatChoiceOrDefault(this ChatCompletions? subject)
    {
        if (subject?.Choices is null || subject.Choices.Count == 0)
        {
            return default;
        }

        return subject.Choices[0];
    }

    public static ChatResponseMessage? GetFirstChatResponseMessageOrDefault(this ChatCompletions? subject)
    {
        return subject.GetFirstChatChoiceOrDefault()?.Message;
    }

    public static ChatCompletionContent GetContent(this Response<ChatCompletions>? subject)
    {
        if (subject is null || !subject.HasValue)
        {
            return new ChatCompletionContent();
        }

        return GetContent(subject.Value);
    }

    public static ChatCompletionContent GetContent(this ChatCompletions? subject)
    {
        if (subject?.Choices is null || subject.Choices.Count == 0)
        {
            return new ChatCompletionContent();
        }

        var firstMessage = subject.GetFirstChatResponseMessageOrDefault();

        return firstMessage is not null ? new ChatCompletionContent(firstMessage.Role, firstMessage.Content) : new ChatCompletionContent();
    }

    public static ChatCompletionResponse GetCompletionResponse(this Response<ChatCompletions>? subject, CommunityRole? communityRole)
    {
        if (subject is null || !subject.HasValue)
        {
            return default;
        }

        return GetCompletionResponse(subject.Value, communityRole ?? CommunityRole.Empty);
    }

    public static ChatCompletionResponse GetCompletionResponse(this ChatCompletions? subject, CommunityRole? communityRole)
    {
        if (subject?.Choices is null || subject.Choices.Count == 0)
        {
            return default;
        }

        return new ChatCompletionResponse(communityRole ?? CommunityRole.Empty, subject.GetContent(), subject.GetMetaData());
    }
}
