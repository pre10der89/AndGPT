using AndGPT.Core.Models;
using FluentAssertions;
using NUnit.Framework;

namespace HeyGPT.Core.Tests.Models;

[TestFixture]
public class ClipboardFormatTests
{
    [Test]
    public void Custom_ShouldThrowArgumentException_WhenFormatIsNull()
    {
        Action act = () => ClipboardFormat.Custom(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Custom_ShouldThrowArgumentException_WhenFormatIsEmpty()
    {
        Action act = () => ClipboardFormat.Custom("");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Custom_ShouldInitializeFormat_WhenValidFormatIsProvided()
    {
        var format = "CustomFormat";
        var clipboardFormat = ClipboardFormat.Custom(format);

        clipboardFormat.Format.Should().Be(format);
    }

    [Test]
    public void ToString_ShouldReturnFormat()
    {
        var format = "CustomFormat";
        var clipboardFormat = ClipboardFormat.Custom(format);

        var result = clipboardFormat.ToString();

        result.Should().Be(format);
    }
}