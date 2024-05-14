using HeyGPT.Core.Models;

namespace HeyGPT.Core.Contracts.Services;

public interface IOpenAIService
{
    Task InitializeAsync();

    Task<ChatCompletionResponse> SendRealCompletion(CommunityMember communityMember, string message, string extraContext="");

    Task<ChatCompletionResponse> SendTestCompletion();
}
