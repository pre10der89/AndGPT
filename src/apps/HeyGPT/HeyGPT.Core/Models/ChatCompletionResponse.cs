﻿using Azure.AI.OpenAI;

namespace HeyGPT.Core.Models;

//TODO: By using the OpenAI types we are now going to force anyone consuming our
//      OpenAIService to carry a transitive dependency on the OpenAI library.
//      It'll likely be too challenging at this time to translate these types into
//      HeyGPT domain models since we don't know yet what we'll need.  So for the
//      time being the OpenAI models are HeyGPT models.

public readonly record struct ChatCompletionResponse
{
    public ChatCompletionResponse()
    {
        CharacterType = CharacterType.Default;
        Content = ChatCompletionContent.Empty;
        MetaData = new ChatCompletionMetaData();
    }

    public ChatCompletionResponse(CharacterType characterType, ChatCompletionContent messageData, ChatCompletionMetaData metaData)
    {
        CharacterType = characterType;
        Content = messageData;
        MetaData = metaData;
    }

    public CharacterType CharacterType { get; init; }

    public ChatCompletionContent Content { get; init; }

    public ChatCompletionMetaData MetaData { get; init; }
}
