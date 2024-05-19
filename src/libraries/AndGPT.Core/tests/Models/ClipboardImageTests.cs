using AndGPT.Core.Models;
using FluentAssertions;
using NUnit.Framework;

namespace HeyGPT.Core.Tests.Models;

[TestFixture]
public class ClipboardImageTests
{
    [Test]
    public void Create_ShouldThrowArgumentException_WhenDataIsNull()
    {
        Action act = () => ClipboardImage.Create(null!, "format");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_ShouldThrowArgumentException_WhenDataIsEmpty()
    {
        Action act = () => ClipboardImage.Create([], "format");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_ShouldInitializeProperties_WhenDataIsValid()
    {
        var data = new byte[] { 1, 2, 3 };
        var format = "format";
        var image = ClipboardImage.Create(data, format);

        image.Data.Should().BeEquivalentTo(data);
        image.Format.Should().Be(format);
    }

    [Test]
    public void ToString_ShouldReturnCorrectString()
    {
        var data = new byte[] { 1, 2, 3 };
        var format = "format";
        var image = ClipboardImage.Create(data, format);

        var result = image.ToString();

        result.Should().Be($"Format: {format}, Size: {data.Length} bytes");
    }
}