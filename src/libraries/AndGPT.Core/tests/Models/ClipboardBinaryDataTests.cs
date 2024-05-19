using AndGPT.Core.Models;
using FluentAssertions;
using NUnit.Framework;

namespace HeyGPT.Core.Tests.Models;

[TestFixture]
public class ClipboardBinaryDataTests
{
    [Test]
    public void Create_ShouldThrowArgumentException_WhenDataIsNull()
    {
        Action act = () => ClipboardBinaryData.Create(null!, "format");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_ShouldThrowArgumentException_WhenDataIsEmpty()
    {
        Action act = () => ClipboardBinaryData.Create(new byte[0], "format");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_ShouldInitializeProperties_WhenDataIsValid()
    {
        var data = new byte[] { 1, 2, 3 };
        var format = "format";
        var binaryData = ClipboardBinaryData.Create(data, format);

        binaryData.Data.Should().BeEquivalentTo(data);
        binaryData.Format.Should().Be(format);
    }

    [Test]
    public void ToString_ShouldReturnCorrectString()
    {
        var data = new byte[] { 1, 2, 3 };
        var format = "format";
        var binaryData = ClipboardBinaryData.Create(data, format);

        var result = binaryData.ToString();

        result.Should().Be($"Format: {format}, Size: {data.Length} bytes");
    }
}