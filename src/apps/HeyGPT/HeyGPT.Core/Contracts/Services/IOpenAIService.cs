using HeyGPT.Core.Models;

namespace HeyGPT.Core.Contracts.Services;

public interface IOpenAIService
{
    Task InitializeAsync();

    Task<ChatCompletionResponse> SendRealCompletion();

    Task<ChatCompletionResponse> SendTestCompletion();
}
