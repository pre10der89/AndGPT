using HeyGPT.Core.Models;

namespace HeyGPT.Core.Contracts.Services;

public interface IOpenAIService
{
    /// <summary>
    /// Simulates a Login to ChatGPT
    /// </summary>
    /// <returns>An asynchronous task.</returns>
    Task LoginAsync();

    /// <summary>
    /// Simulates a Logout from ChatGPT
    /// </summary>
    /// <returns>An asynchronous task.</returns>
    Task LogoutAsync();

    /// <summary>
    /// Send a prompt to ChatGPT with the specified user message.
    /// </summary>
    /// <param name="message">The user message.</param>
    /// <param name="extraContext">Any additional context.</param>
    /// <returns>An asynchronous task.</returns>
    Task<ChatCompletionResponse> SendPrompt(string message, string extraContext = "");

    /// <summary>
    /// Send a prompt to ChatGPT with the specified characteristics.
    /// </summary>
    /// <param name="chatCharacterDetails">Details about the persona ChatGPT should adopt.</param>
    /// <param name="message">The user message.</param>
    /// <param name="extraContext">Any additional context.</param>
    /// <returns>An asynchronous task.</returns>
    Task<ChatCompletionResponse> SendPrompt(ChatCharacterDetails chatCharacterDetails, string message, string extraContext="");

    /// <summary>
    /// Temporary method used to test how certain responses are rendered by the UX.
    /// </summary>
    /// <param name="response">The response to be reflected.</param>
    /// <returns>An asynchronous task.</returns>
    Task<ChatCompletionResponse> SendPromptWithCannedResponse(ChatCompletionResponse response);
}
