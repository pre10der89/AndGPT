using FluentAssertions;
using HeyGPT.Core.Models;
using NUnit.Framework;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace HeyGPT.Core.Tests;

[TestFixture]
public class OpenAISecretKeyTests
{
    [Test]
    public void Empty_ShouldInitializeProperties_ToEmptyValues()
    {
        var key = OpenAISecretKey.Empty;

        key.Value.Should().BeEmpty();
        key.ObfuscateValue.Should().BeEmpty();
        key.IsEmpty.Should().BeTrue();
    }

    [Test]
    public void Default_Constructor_ShouldInitializeProperties_ToEmptyValues()
    {
        var key = new OpenAISecretKey();

        key.Value.Should().BeEmpty();
        key.ObfuscateValue.Should().BeEmpty();
        key.IsEmpty.Should().BeTrue();
    }

    [Test]
    public void Constructor_ShouldInitializeProperties_WhenValidInput()
    {
        var key = new OpenAISecretKey("sk-proj-123456789");

        key.Value.Should().BeEquivalentTo("sk-proj-123456789");
        key.ObfuscateValue.Should().BeEquivalentTo("sk-proj-***456789");

        key.IsEmpty.Should().BeFalse();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenNullInput()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new OpenAISecretKey(null));
    }

    [Test]
    public void Constructor_ShouldHandleWhitespace_WhenWhitespaceInput()
    {
        Assert.Throws<ArgumentNullException>(() => new OpenAISecretKey("   "));
    }

    [Test]
    public void ImplicitOperator_ShouldReturnStringValue()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var key = new OpenAISecretKey("sk-proj-123456789");
            var result = (string)key;
        });
    }

    [Test]
    public void ExplicitOperator_ShouldCreateInstanceFromString()
    {
        var key = (OpenAISecretKey)"sk-proj-123456789";
        key.Value.Should().BeEquivalentTo("sk-proj-123456789");
    }

    [Test]
    public void ObfuscateString_ShouldObfuscateCorrectly()
    {
        var obfuscated = OpenAISecretKey.ObfuscateString("sk-proj-123456789");
        obfuscated.Should().BeEquivalentTo("sk-proj-***456789");
    }

    [Test]
    public void ObfuscateString_ShouldHandleNonPrefixStrings()
    {
        var obfuscated = OpenAISecretKey.ObfuscateString("123456789");
        obfuscated.Should().BeEquivalentTo("***456789");
    }

    [Test]
    public void ObfuscateString_ShouldHandleEmptyStrings()
    {
        var obfuscated = OpenAISecretKey.ObfuscateString("");

        obfuscated.Should().BeEquivalentTo("");
    }

    [Test]
    public void ObfuscateString_ShouldHandleNullStrings()
    {
        var obfuscated = OpenAISecretKey.ObfuscateString(null);
        obfuscated.Should().BeEquivalentTo("");
    }

    [Test]
    public void ToString_Should_Return_Obfuscated_Value()
    {
        const string input = @"sk-proj-123456789";

        var obfuscated = OpenAISecretKey.ObfuscateString(input);

        var key = new OpenAISecretKey(input);

        obfuscated.Should().BeEquivalentTo(key.ToString());
    }

    [TestCase("sk-proj-123456789", ExpectedResult = "sk-proj-***456789")]
    [TestCase("sk-proj-12345", ExpectedResult = "sk-proj-12345")]
    [TestCase("sk-proj-", ExpectedResult = "sk-proj-")]
    [TestCase("123456789", ExpectedResult = "***456789")]
    [TestCase("12345", ExpectedResult = "12345")]
    [TestCase("", ExpectedResult = "")]
    [TestCase(null, ExpectedResult = "")]
    public string TestObfuscateString(string input)
    {
        return OpenAISecretKey.ObfuscateString(input);
    }
}