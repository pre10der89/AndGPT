using AndGPT.Core.Models;
using FluentAssertions;
using NUnit.Framework;

namespace HeyGPT.Core.Tests.Models;

[TestFixture]
public class ClipboardTextTests
{
    [Test]
    public void Create_ShouldThrowArgumentNullException_WhenTextIsNull()
    {
        Action act = () => ClipboardText.Create(null!);
        act.Should().Throw<ArgumentNullException>().WithMessage("*text*");
    }

    [Test]
    public void Create_ShouldInitializeProperties_WhenTextIsValid()
    {
        var text = "Hello, world!";
        var clipboardText = ClipboardText.Create(text);

        clipboardText.Text.Should().Be(text);
        clipboardText.Format.Should().BeEmpty();
    }

    [Test]
    public void Create_WithFormat_ShouldThrowArgumentNullException_WhenTextIsNull()
    {
        Action act = () => ClipboardText.Create(null!, "format");
        act.Should().Throw<ArgumentNullException>().WithMessage("*text*");
    }

    [Test]
    public void Create_WithFormat_ShouldInitializeProperties_WhenTextIsValid()
    {
        var text = "Hello, world!";
        var format = "CustomFormat";
        var clipboardText = ClipboardText.Create(text, format);

        clipboardText.Text.Should().Be(text);
        clipboardText.Format.Should().Be(format);
    }

    [Test]
    public void Create_WithFormat_ShouldInitializeEmptyFormat_WhenFormatIsNull()
    {
        var text = "Hello, world!";
        var clipboardText = ClipboardText.Create(text, null!);

        clipboardText.Text.Should().Be(text);
        clipboardText.Format.Should().BeEmpty();
    }

    [Test]
    public void Empty_ShouldReturnEmptyClipboardText()
    {
        var emptyClipboardText = ClipboardText.Empty;

        emptyClipboardText.Text.Should().BeEmpty();
        emptyClipboardText.Format.Should().BeEmpty();
    }

    [Test]
    public void IsEmpty_ShouldReturnTrue_WhenTextIsEmpty()
    {
        var emptyClipboardText = ClipboardText.Empty;

        emptyClipboardText.IsEmpty.Should().BeTrue();
    }

    [Test]
    public void IsEmpty_ShouldReturnFalse_WhenTextIsNotEmpty()
    {
        var text = "Hello, world!";
        var clipboardText = ClipboardText.Create(text);

        clipboardText.IsEmpty.Should().BeFalse();
    }

    [Test]
    public void ToString_ShouldReturnCorrectString()
    {
        var text = "Hello, world!";
        var format = "CustomFormat";
        var clipboardText = ClipboardText.Create(text, format);

        var result = clipboardText.ToString();

        result.Should().Be($"Format: {format}, Length: {text.Length} bytes");
    }
}