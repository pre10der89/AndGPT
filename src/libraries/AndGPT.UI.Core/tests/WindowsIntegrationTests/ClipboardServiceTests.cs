using AndGPT.Core.Models;
using AndGPT.UI.Core.Services;
using FluentAssertions;
using NUnit.Framework;

namespace WindowsIntegrationTests;

[TestFixture, RequiresThread(ApartmentState.STA)]
public class WindowsClipboardServiceTests
{
    private WindowsClipboardService? _clipboardService;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _clipboardService = new WindowsClipboardService();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _clipboardService?.Dispose();
    }

    [Test]
    public async Task SetTextAsync_ShouldSetAndGetText()
    {
        var text = ClipboardText.Create("Hello, Clipboard!");

        var result = await _clipboardService!.SetTextAsync(text);
        result.Should().BeTrue();

        var clipboardText = await _clipboardService.GetTextAsync();
        clipboardText.Text.Should().Be(text.Text);
    }

    [Test]
    public async Task SetUriAsync_ShouldSetAndGetUri()
    {
        var uri = new Uri("http://example.com");

        var result = await _clipboardService!.SetUriAsync(uri);
        result.Should().BeTrue();

        var clipboardUri = await _clipboardService.GetUriAsync();
        clipboardUri.Should().Be(uri);
    }

    [Test]
    public async Task SetImageAsync_ShouldSetAndGetImage()
    {
        var imageData = new byte[] { 1, 2, 3, 4, 5 };
        var image = ClipboardImage.Create(imageData, "Bitmap");

        var result = await _clipboardService!.SetImageAsync(image);
        result.Should().BeTrue();

        var clipboardImage = await _clipboardService.GetImageAsync();
        clipboardImage.Data.Should().BeEquivalentTo(imageData);
    }

    [Test]
    public async Task SetBinaryAsync_ShouldSetAndGetBinaryData()
    {
        var binaryData = ClipboardBinaryData.Create(new byte[] { 1, 2, 3, 4, 5 }, "CustomFormat");

        var result = await _clipboardService!.SetBinaryAsync(binaryData);
        result.Should().BeTrue();

        var clipboardBinaryData = await _clipboardService.GetBinaryAsync(ClipboardFormat.Custom("CustomFormat"));
        clipboardBinaryData.Data.Should().BeEquivalentTo(binaryData.Data);
    }

    [Test]
    public async Task GetClipboardDataType_ShouldReturnCorrectDataType()
    {
        var text = ClipboardText.Create("Hello, Clipboard!");
        await _clipboardService!.SetTextAsync(text);
        var dataType = _clipboardService.GetClipboardDataType();
        dataType.Should().Be(ClipboardDataType.Text);

        var imageData = new byte[] { 1, 2, 3, 4, 5 };
        var image = ClipboardImage.Create(imageData, "Bitmap");
        await _clipboardService.SetImageAsync(image);
        dataType = _clipboardService.GetClipboardDataType();
        dataType.Should().Be(ClipboardDataType.Image);

        var uri = new Uri("http://example.com");
        await _clipboardService.SetUriAsync(uri);
        dataType = _clipboardService.GetClipboardDataType();
        dataType.Should().Be(ClipboardDataType.Uri);
    }
}
