using System.Runtime.CompilerServices;
using AndGPT.Core.Contracts.Services;
using Azure;
using Azure.AI.OpenAI;
using HeyGPT.Core.Contracts.Services;
using HeyGPT.Core.Extensions;
using HeyGPT.Core.Models;

namespace HeyGPT.Core.Services;

public class OpenAIService : IOpenAIService
{
    private const string ApiKeyEnvVariableName = @"HeyGPTKey";
    private const string ApiOrganizationEnvVariableName = @"HeyGPTOrganization";

    private readonly IEnvironmentVariableService _environmentVariableService;

    private bool _useAzureOpenAI = false;
    private OpenAIClient? _openAIClient;
    private OpenAISecretKey _secretKey = OpenAISecretKey.Empty;
    private string _organizationId = string.Empty;

    // ReSharper disable once ConvertToPrimaryConstructor
    public OpenAIService(IEnvironmentVariableService environmentVariableService)
    {
        _useAzureOpenAI = false;

        _environmentVariableService = environmentVariableService;
    }

    public async Task InitializeAsync()
    {
        // TODO: Use User Secrets
        var secretKey = _environmentVariableService.GetEnvironmentVariable(ApiKeyEnvVariableName);

        _useAzureOpenAI = false;
        _secretKey = new OpenAISecretKey(secretKey);
        _organizationId = _environmentVariableService.GetEnvironmentVariable(ApiOrganizationEnvVariableName);


        //_openAIClient = _useAzureOpenAI
        //    ? new OpenAIClient(
        //        new Uri("https://your-azure-openai-resource.com/"),
        //        new AzureKeyCredential("your-azure-openai-resource-api-key"))
        //    : new OpenAIClient("your-api-key-from-platform.openai.com");

        // TODO: Determine if we should maintain an instance of the OpenAIClient or whether this is something you do for each round...
        // TODO: We may consider wiping the secret key immediate after we create the OpenAIClient.  Is there any reason to keep it around?      

        _openAIClient = new OpenAIClient(_secretKey.Value);

        await Task.CompletedTask;
    }

    public async Task<ChatCompletionResponse> SendRealCompletion(CommunityMember communityMember, string message, string extraContext = "")
    {
        _openAIClient ??= new OpenAIClient(_secretKey.Value);

        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = "gpt-3.5-turbo", //"gpt-4", // Use DeploymentName for "model" with non-Azure clients
            Messages =
            {
                // The system message represents instructions or other guidance about how the assistant should behave
                new ChatRequestSystemMessage($"You are a helpful assistant. You are a {communityMember.CommunityRole.Value}"),
                // new ChatRequestUserMessage("Can you help me?")
            }
        };

        if (communityMember.Pithy)
        {
            chatCompletionsOptions.Messages.Add(new ChatRequestSystemMessage("Be pithy in your answer, I'm paying for this!"));
        }

        foreach (var prompt in communityMember.CharacterPrompts)
        {
            chatCompletionsOptions.Messages.Add(new ChatRequestSystemMessage(prompt));
        }

        chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(message));

        if (!string.IsNullOrEmpty(extraContext))
        {
            chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(extraContext));
        }

        chatCompletionsOptions.Temperature = (float)communityMember.Temperature;

        var response = await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);

        return response.GetCompletionResponse(communityMember.CommunityRole);
    }
    public async Task<ChatCompletionResponse> SendRealCompletion2(CommunityMember communityMember, string message)
    {
        _openAIClient ??= new OpenAIClient(_secretKey.Value);

        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = "gpt-3.5-turbo", //"gpt-4", // Use DeploymentName for "model" with non-Azure clients
            Messages =
            {
                // The system message represents instructions or other guidance about how the assistant should behave
                new ChatRequestSystemMessage("You are a helpful assistant. You will talk like a pirate."),
                new ChatRequestSystemMessage("Be concise in your answer, I'm paying for this!"),
                // User messages represent current or historical input from the end user
                new ChatRequestUserMessage("Can you help me?"),
                // Assistant messages represent historical responses from the assistant
                //new ChatRequestAssistantMessage("Arrrr! Of course, me hearty! What can I do for ye?"),
                //new ChatRequestUserMessage("What's the best way to train a parrot?"),
                //new ChatRequestUserMessage("Can you write me a C# method only a pirate would love?"),
                //new ChatRequestUserMessage("Can you create me a table of pirate booty?"),
                new ChatRequestUserMessage(message),
            }
        };

        var response = await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);

        return response.GetCompletionResponse(new CommunityRole("Pirate"));
    }

    public async Task<ChatCompletionResponse> SendTestCompletion()
    {
        return GetCannedResponse();

        _openAIClient ??= new OpenAIClient(_secretKey.Value);

        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = "gpt-3.5-turbo", // Use DeploymentName for "model" with non-Azure clients
            Messages =
            {
                // The system message represents instructions or other guidance about how the assistant should behave
                new ChatRequestSystemMessage("You are a helpful assistant. You will talk like a pirate."),
                new ChatRequestSystemMessage("Be concise in your answer, I'm paying for this!"),
                // User messages represent current or historical input from the end user
                new ChatRequestUserMessage("Can you help me?"),
                // Assistant messages represent historical responses from the assistant
                //new ChatRequestAssistantMessage("Arrrr! Of course, me hearty! What can I do for ye?"),
                //new ChatRequestUserMessage("What's the best way to train a parrot?"),
                new ChatRequestUserMessage("Can you write me a C# method only a pirate would love?"),
                new ChatRequestUserMessage("Can you create me a table of pirate booty?"),
            }
        };

        var response = await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);

        return response.GetCompletionResponse(new CommunityRole("Pirate"));
    }

    private ChatCompletionResponse GetCannedResponse()
    {
        var cannedMessage =
            @"Arr matey, I be happy to oblige ye with a fine piece of code that'll make even the saltiest pirate crack a smile. Here be a C# method that be fit for a scurvy sea dog:

```csharp
using System;

class PirateHelper
{
    public void SwabTheDeck()
    {
        Console.WriteLine(""Avast ye! Swabbin' the deck, arr!"");
    }

    public void RaiseTheJollyRoger()
    {
        Console.WriteLine(""Hoist the Jolly Roger, ye landlubbers!"");
    }

    public void DrinkUpMeHearties()
    {
        Console.WriteLine(""Drink up me hearties, yo ho!"");
    }
}
```

Run this code in your C# application and ye'll be sailin' the high seas in no time! Arrr! [228]";

        return new ChatCompletionResponse(new CommunityRole("Pirate"), new ChatCompletionContent(ChatRole.Assistant, cannedMessage), new ChatCompletionMetaData());
    }
}
