using AndGPT.Core.Contracts.Services;
using Azure.AI.OpenAI;
using HeyGPT.Core.Contracts.Services;
using HeyGPT.Core.Exceptions;
using HeyGPT.Core.Extensions;
using HeyGPT.Core.Models;

namespace HeyGPT.Core.Services;

// TODO: We have a bunch of English hard-coded values in this file.  The string's repository is part of the application while this service is in a different assembly.
//       In the real world, we'd need to send in localized versions of the system prompts that we hardcode here.  This could be done by injecting the resource
//       service or by adding a configuration method where the built-in prompts are set.   Need to think about this more... 

// TODO: The code that is building the ChatCompletionOptions are private methods making them hard to test.  We should probably create a builder
//       that will allow us to test the relationship with ChatCharacterDetails.  Might also solve the localization issue stated above.

public class OpenAIService : IOpenAIService
{
    #region Constants

    private const string ApiKeyEnvVariableName = "HeyGPTKey";
    private const string ApiOrganizationEnvVariableName = "HeyGPTOrganization";
    private const string DefaultModelVersion = "gpt-3.5-turbo"; //"gpt-4", "gpt-4-turbo

    #endregion

    #region Fields

    private readonly IEnvironmentVariableService _environmentVariableService;

    private OpenAIClient? _openAIClient;

    #endregion

    #region Constructor(s)

    // ReSharper disable once ConvertToPrimaryConstructor
    public OpenAIService(IEnvironmentVariableService environmentVariableService)
    {
        _environmentVariableService = environmentVariableService;
    }

    #endregion

    #region IOpenAIService Members

    public async Task LoginAsync()
    {
        CheckLoggedIn();

        await Task.CompletedTask;
    }

    public async Task LogoutAsync()
    {
        _openAIClient = null;

        await Task.CompletedTask;
    }

    public async Task<ChatCompletionResponse> SendPrompt(string message, string extraContext = "")
    {
        return await SendPrompt(new ChatCharacterDetails(), message, extraContext);
    }

    public async Task<ChatCompletionResponse> SendPrompt(ChatCharacterDetails chatCharacterDetails, string message, string extraContext = "")
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("A message is required!", nameof(message));
        }

        CheckLoggedIn();

        var chatCompletionsOptions = new ChatCompletionsOptions
        {
            DeploymentName = DefaultModelVersion
        };


        SetPromptOptions(chatCompletionsOptions, chatCharacterDetails);

        AddCharacterTraits(chatCompletionsOptions, chatCharacterDetails);

        PreprocessAndAddUserMessage(chatCompletionsOptions, message, extraContext);

        var response = await _openAIClient!.GetChatCompletionsAsync(chatCompletionsOptions);

        return response.GetCompletionResponse(chatCharacterDetails.CharacterType);
    }


    public async Task<ChatCompletionResponse> SendPromptWithCannedResponse(ChatCompletionResponse response)
    {
        // TODO: Remove this once we have a better idea how all the different responses are rendered.
        //       We don't want to waste tokens on the API account testing how a message is rendered.

        await Task.CompletedTask;

        return response;
    }

    #endregion

    #region Private Methods

    private void CheckLoggedIn()
    {
        if (_openAIClient is not null)
        {
            return;
        }

        try
        {
            // TODO: Use User Secrets leaving the ENV method as a backup.  Should be able to set the local settings with the user secrets as well.
            // TODO: Ultimately, we'd want to do some real OAuth here... 
            var secretKey = new OpenAISecretKey(_environmentVariableService.GetEnvironmentVariable(ApiKeyEnvVariableName));

            _openAIClient = new OpenAIClient(secretKey.Value);
        }
        catch (Exception ex)
        {
            throw new OpenAIInitializationException(
                "Failed to initialize the OpenAIClient - Ensure you have set the 'HeyGPTKey' Environment Variable with a valid OpenAI API Key", ex);
        }
    }

    private void AppendExtraContentToPrompt(ChatCompletionsOptions chatCompletionsOptions, string extraContext)
    {
        // TODO: This is only a POC right now... The idea is that we can push content to the next prompt from the clipboard.  This was seen in the
        //       OpenAI Spring Update demos regarding the Mac desktop application where they copied text to the clipboard and sent with the prompt.
        //       It is unclear what the semantics of that feature are

        if (!string.IsNullOrEmpty(extraContext))
        {
            chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(extraContext));
        }
    }

    private void PreprocessAndAddUserMessage(ChatCompletionsOptions chatCompletionsOptions, string message, string extraContext)
    {
        // TODO: Determine what sort of pre-processing is done in ChatGPT... We may want to examine
        // the message to add custom inferred context. ChatGPT probably does something like this to
        // determine whether to call DALL-E or just a text prompt.

        // Add the prompt entered by the user...
        chatCompletionsOptions.AddUserMessage(message);

        AppendExtraContentToPrompt(chatCompletionsOptions, extraContext);
    }

    private void SetPromptOptions(ChatCompletionsOptions chatCompletionsOptions, ChatCharacterDetails chatCharacterDetails)
    {
        // Set how crazy the responses should be :)
        chatCompletionsOptions.SetCompletionTemperature(chatCharacterDetails.Temperature);

        // If the user wants the responses to be pithy or concise (to save money) then this flag should be set.  This will tell ChatGPT to use less tokens!
        if (chatCharacterDetails.ShouldBePithy)
        {
            chatCompletionsOptions.Messages.Add(new ChatRequestSystemMessage("Be pithy in your answer!"));
        }
    }

    private void AddCharacterTraits(ChatCompletionsOptions chatCompletionsOptions, ChatCharacterDetails chatCharacterDetails)
    {
        if (!chatCharacterDetails.CharacterType.IsSpecified)
        {
            return;
        }

        // Top-Level System Prompt
        chatCompletionsOptions.AddSystemMessage("You are a helpful assistant.");

        // If a character type has been specified we will add a system message instructing ChatGPT to take on the specified personality. 
        chatCompletionsOptions.Messages.Add(new ChatRequestSystemMessage($"You should act like a {chatCharacterDetails.CharacterType.Value}"));

        // If there are any additional characteristics then they will be sent to further instruct ChatGPT how to response.
        // TODO: Determine whether this is better to be a UserMessage?
        chatCompletionsOptions.AddSystemMessages(chatCharacterDetails.AdditionalCharacteristics);
    }

    // TODO: Move to Application Service
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

        return new ChatCompletionResponse(new CharacterType("Pirate"), new ChatCompletionContent(ChatRole.Assistant, cannedMessage), new ChatCompletionMetaData());
    }

    #endregion
}
