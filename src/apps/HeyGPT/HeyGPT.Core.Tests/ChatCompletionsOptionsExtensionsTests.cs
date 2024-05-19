using Azure.AI.OpenAI;
using FluentAssertions;
using NUnit.Framework;
using HeyGPT.Core.Extensions;

namespace HeyGPT.Core.Tests;

public class ChatCompletionsOptionsExtensionsTests
{
    [Test]
    public void SetCompletionTemperature_ShouldSetTemperature_WhenValueIsProvided()
    {
        // Arrange
        var options = new ChatCompletionsOptions();
        var expectedTemperature = 0.7f;

        // Act
        options.SetCompletionTemperature(expectedTemperature);

        // Assert
        options.Temperature.Should().Be(expectedTemperature);
    }

    [Test]
    public void SetCompletionTemperature_ShouldNotSetTemperature_WhenValueIsNull()
    {
        // Arrange
        var options = new ChatCompletionsOptions();
        float? value = null;

        // Act
        options.SetCompletionTemperature(value);

        // Assert
        options.Temperature.Should().BeNull();
    }

    [Test]
    public void AddSystemMessage_ShouldAddMessage_WhenMessageIsValid()
    {
        // Arrange
        var options = new ChatCompletionsOptions();
        var message = "System message";

        // Act
        options.AddSystemMessage(message);

        // Assert
        options.Messages.Should().ContainSingle()
            .Which.Should().BeOfType<ChatRequestSystemMessage>()
            .Which.Content.Should().Be(message);
    }

    [Test]
    public void AddSystemMessages_ShouldAddMessages_WhenMessagesAreValid()
    {
        // Arrange
        var options = new ChatCompletionsOptions();
        var messages = new List<string> { "Message 1", "Message 2" };

        // Act
        options.AddSystemMessages(messages);

        // Assert
        options.Messages.Should().HaveCount(2);
        options.Messages[0].Should().BeOfType<ChatRequestSystemMessage>().Which.Content.Should().Be("Message 1");
        options.Messages[1].Should().BeOfType<ChatRequestSystemMessage>().Which.Content.Should().Be("Message 2");
    }

    [Test]
    public void AddUserMessage_ShouldAddMessage_WhenMessageIsValid()
    {
        // Arrange
        var options = new ChatCompletionsOptions();
        var message = "User message";

        // Act
        options.AddUserMessage(message);

        // Assert
        options.Messages.Should().ContainSingle()
            .Which.Should().BeOfType<ChatRequestUserMessage>()
            .Which.Content.Should().Be(message);
    }

    [Test]
    public void AddUserMessages_ShouldAddMessages_WhenMessagesAreValid()
    {
        // Arrange
        var options = new ChatCompletionsOptions();
        var messages = new List<string> { "User message 1", "User message 2" };

        // Act
        options.AddUserMessages(messages);

        // Assert
        options.Messages.Should().HaveCount(2);
        options.Messages[0].Should().BeOfType<ChatRequestUserMessage>().Which.Content.Should().Be("User message 1");
        options.Messages[1].Should().BeOfType<ChatRequestUserMessage>().Which.Content.Should().Be("User message 2");
    }
}