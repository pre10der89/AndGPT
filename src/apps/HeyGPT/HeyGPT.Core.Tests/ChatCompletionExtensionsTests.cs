using Azure;
using Azure.AI.OpenAI;
using Azure.Core;
using FluentAssertions;
using HeyGPT.Core.Extensions;
using HeyGPT.Core.Models;
using NUnit.Framework;

namespace HeyGPT.Core.Tests;

[TestFixture]
public class ChatCompletionExtensionsTests
{
    [Test]
    public void GetTokenUsage_ShouldReturnEmpty_WhenSubjectIsNull()
    {
        // Arrange
        Response<ChatCompletions>? subject = null;

        // Act
        var result = subject.GetTokenUsage();

        // Assert
        result.Should().Be(ChatTokenUsage.Empty);
    }

    //[Test]
    //public void GetTokenUsage_ShouldReturnUsage_WhenValidSubjectProvided()
    //{
    //    // Arrange
    //    var usage = new CompletionsUsage(1, 2, 3);
    //    var chatCompletions = new ChatCompletions { Usage = usage };
    //    var response = Response.FromValue(chatCompletions, new MockResponse(200));

    //    // Act
    //    var result = response.GetTokenUsage();

    //    // Assert
    //    result.Should().BeEquivalentTo(new ChatTokenUsage(1, 2, 3));
    //}

    [Test]
    public void GetCompletionId_ShouldReturnEmpty_WhenSubjectIsNull()
    {
        // Arrange
        Response<ChatCompletions>? subject = null;

        // Act
        var result = subject.GetCompletionId();

        // Assert
        result.Should().Be(string.Empty);
    }

    //[Test]
    //public void GetCompletionId_ShouldReturnId_WhenValidSubjectProvided()
    //{
    //    // Arrange
    //    var chatCompletions = new ChatCompletions { Id = "test_id" };
    //    var response = Response.FromValue(chatCompletions, new MockResponse(200));

    //    // Act
    //    var result = response.GetCompletionId();

    //    // Assert
    //    result.Should().Be("test_id");
    //}

    [Test]
    public void GetModel_ShouldReturnEmpty_WhenSubjectIsNull()
    {
        // Arrange
        Response<ChatCompletions>? subject = null;

        // Act
        var result = subject.GetModel();

        // Assert
        result.Should().Be(string.Empty);
    }

    //[Test]
    //public void GetModel_ShouldReturnModel_WhenValidSubjectProvided()
    //{
    //    // Arrange
    //    var chatCompletions = new ChatCompletions { Model = "test_model" };
    //    var response = Response.FromValue(chatCompletions, new MockResponse(200));

    //    // Act
    //    var result = response.GetModel();

    //    // Assert
    //    result.Should().Be("test_model");
    //}

    [Test]
    public void GetCreationDate_ShouldReturnMinValue_WhenSubjectIsNull()
    {
        // Arrange
        Response<ChatCompletions>? subject = null;

        // Act
        var result = subject.GetCreationDate();

        // Assert
        result.Should().Be(DateTimeOffset.MinValue);
    }

    //[Test]
    //public void GetCreationDate_ShouldReturnDate_WhenValidSubjectProvided()
    //{
    //    // Arrange
    //    var chatCompletions = new ChatCompletions { Created = DateTimeOffset.UtcNow };
    //    var response = Response.FromValue(chatCompletions, new MockResponse(200));

    //    // Act
    //    var result = response.GetCreationDate();

    //    // Assert
    //    result.Should().Be(chatCompletions.Created);
    //}

    [Test]
    public void GetMetaData_ShouldReturnDefault_WhenSubjectIsNull()
    {
        // Arrange
        Response<ChatCompletions>? subject = null;

        // Act
        var result = subject.GetMetaData();

        // Assert
        result.Should().BeEquivalentTo(new ChatCompletionMetaData());
    }

    //[Test]
    //public void GetMetaData_ShouldReturnMetaData_WhenValidSubjectProvided()
    //{
    //    // Arrange
    //    var chatCompletions = new ChatCompletions
    //    {
    //        Id = "test_id",
    //        Model = "test_model",
    //        Created = DateTimeOffset.UtcNow,
    //        Usage = new CompletionsUsage { CompletionTokens = 1, PromptTokens = 2, TotalTokens = 3 }
    //    };
    //    var response = Response.FromValue(chatCompletions, new MockResponse(200));

    //    // Act
    //    var result = response.GetMetaData();

    //    // Assert
    //    result.Should().BeEquivalentTo(new ChatCompletionMetaData(
    //        chatCompletions.Id,
    //        chatCompletions.Model,
    //        chatCompletions.Created,
    //        new ChatTokenUsage(1, 2, 3)));
    //}

    [Test]
    public void GetChatChoicesList_ShouldReturnEmptyList_WhenSubjectIsNull()
    {
        // Arrange
        ChatCompletions? subject = null;

        // Act
        var result = subject.GetChatChoicesList();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void GetFirstChatChoiceOrDefault_ShouldReturnNull_WhenSubjectIsNull()
    {
        // Arrange
        ChatCompletions? subject = null;

        // Act
        var result = subject.GetFirstChatChoiceOrDefault();

        // Assert
        result.Should().BeNull();
    }

    // Add more tests for remaining methods
}

// Mock Response class for testing purposes
public class MockResponse : Response
{
    private readonly int _status;

    public MockResponse(int status)
    {
        _status = status;
    }

    public override int Status => _status;

    public override string ReasonPhrase => throw new NotImplementedException();
    public override Stream? ContentStream
    {
        get => throw new NotImplementedException(); set => throw new NotImplementedException();
    }
    public override string ClientRequestId
    {
        get => throw new NotImplementedException(); set => throw new NotImplementedException();
    }

    public override void Dispose()
    {
        throw new NotImplementedException();
    }

    protected override bool TryGetHeader(string name, out string value)
    {
        throw new NotImplementedException();
    }

    protected override bool TryGetHeaderValues(string name, out IEnumerable<string> values)
    {
        throw new NotImplementedException();
    }

    protected override bool ContainsHeader(string name)
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<HttpHeader> EnumerateHeaders()
    {
        throw new NotImplementedException();
    }
}