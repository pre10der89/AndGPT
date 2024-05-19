using AndGPT.Core.Models;
using FluentAssertions;
using NUnit.Framework;

namespace HeyGPT.Core.Tests.Models;

[TestFixture]
public class ClipboardContentTests
{
    [Test]
    public void CreateTextContent_ShouldInitializeProperties()
    {
        var text = "Hello, world!";
        var content = ClipboardContent.CreateTextContent(ClipboardText.Create(text));

        content.DataType.Should().Be(ClipboardDataType.Text);
        content.Text!.Text.Should().Be(text);
        content.Image.Should().BeNull();
        content.Uri.Should().BeNull();
        content.Binary.Should().BeNull();
    }

    [Test]
    public void CreateImageContent_ShouldInitializeProperties()
    {
        var imageData = new byte[] { 1, 2, 3 };
        var image = ClipboardImage.Create(imageData, "Bitmap");
        var content = ClipboardContent.CreateImageContent(image);

        content.DataType.Should().Be(ClipboardDataType.Image);
        content.Image.Should().Be(image);
        content.Text.Should().BeNull();
        content.Uri.Should().BeNull();
        content.Binary.Should().BeNull();
    }

    [Test]
    public void CreateUriContent_ShouldInitializeProperties()
    {
        var uri = new Uri("http://example.com");
        var content = ClipboardContent.CreateUriContent(uri);

        content.DataType.Should().Be(ClipboardDataType.Uri);
        content.Uri.Should().Be(uri);
        content.Text.Should().BeNull();
        content.Image.Should().BeNull();
        content.Binary.Should().BeNull();
    }

    [Test]
    public void CreateBinaryContent_ShouldInitializeProperties()
    {
        var binaryData = ClipboardBinaryData.Create(new byte[] { 1, 2, 3 }, "format");
        var content = ClipboardContent.CreateBinaryContent(binaryData);

        content.DataType.Should().Be(ClipboardDataType.Binary);
        content.Binary.Should().Be(binaryData);
        content.Text.Should().BeNull();
        content.Image.Should().BeNull();
        content.Uri.Should().BeNull();
    }
}