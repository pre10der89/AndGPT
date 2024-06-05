using System.Text;
using FluentAssertions;
using NUnit.Framework;
using AndGPT.Core.Contracts.Services;
using AndGPT.UI.Core.Services;
using AndGPT.Core.Exceptions;

namespace WindowsInfrastructure.IntegrationTests;

[TestFixture]
public class WindowsDataProtectionServiceTests
{
    private IDataProtectionService _dataProtectionService;

    [SetUp]
    public void Setup()
    {
        _dataProtectionService = new WindowsDataProtectionService();
    }

    [Test]
    public async Task EncryptAsync_ShouldReturnEncryptedData()
    {
        // Arrange
        var originalData = "Test data"u8.ToArray();

        // Act
        var encryptedData = await _dataProtectionService.EncryptAsync(originalData);

        // Assert
        encryptedData.Should().NotBeNullOrEmpty();
        encryptedData.Should().NotEqual(originalData);
    }

    [Test]
    public async Task EncryptAsync_WithAdditionalEntropy_ShouldReturnEncryptedData()
    {
        // Arrange
        var originalData = "Test data"u8.ToArray();
        var additionalEntropy = "Entropy"u8.ToArray();

        // Act
        var encryptedData = await _dataProtectionService.EncryptAsync(originalData, additionalEntropy);

        // Assert
        encryptedData.Should().NotBeNullOrEmpty();
        encryptedData.Should().NotEqual(originalData);
    }

    [Test]
    public async Task DecryptAsync_ShouldReturnOriginalData()
    {
        // Arrange
        var originalData = "Test data"u8.ToArray();
        var encryptedData = await _dataProtectionService.EncryptAsync(originalData);

        // Act
        var decryptedData = await _dataProtectionService.DecryptAsync(encryptedData);

        // Assert
        decryptedData.Should().Equal(originalData);
    }

    [Test]
    public async Task DecryptAsync_WithAdditionalEntropy_ShouldReturnOriginalData()
    {
        // Arrange
        var originalData = "Test data"u8.ToArray();
        var additionalEntropy = "Entropy"u8.ToArray();
        var encryptedData = await _dataProtectionService.EncryptAsync(originalData, additionalEntropy);

        // Act
        var decryptedData = await _dataProtectionService.DecryptAsync(encryptedData, additionalEntropy);

        // Assert
        decryptedData.Should().Equal(originalData);
    }

    [Test]
    public async Task EncryptToBase64Async_ShouldReturnBase64String()
    {
        // Arrange
        var originalData = "Test data"u8.ToArray();

        // Act
        var base64String = await _dataProtectionService.EncryptToBase64Async(originalData);

        // Assert
        base64String.Should().NotBeNullOrEmpty();
        base64String.Should().NotBe(Convert.ToBase64String(originalData));
    }

    [Test]
    public async Task DecryptFromBase64Async_ShouldReturnOriginalData()
    {
        // Arrange
        var originalData = "Test data"u8.ToArray();
        var base64String = await _dataProtectionService.EncryptToBase64Async(originalData);

        // Act
        var decryptedData = await _dataProtectionService.DecryptFromBase64Async(base64String);

        // Assert
        decryptedData.Should().Equal(originalData);
    }

    [Test]
    public async Task EncryptStringToBase64Async_ShouldReturnBase64String()
    {
        // Arrange
        const string originalString = "Test data";

        // Act
        var base64String = await _dataProtectionService.EncryptStringToBase64Async(originalString);

        // Assert
        base64String.Should().NotBeNullOrEmpty();
        base64String.Should().NotBe(Convert.ToBase64String(Encoding.UTF8.GetBytes(originalString)));
    }

    [Test]
    public async Task DecryptStringFromBase64Async_ShouldReturnOriginalString()
    {
        // Arrange
        const string originalString = "Test data";
        var base64String = await _dataProtectionService.EncryptStringToBase64Async(originalString);

        // Act
        var decryptedString = await _dataProtectionService.DecryptStringFromBase64Async(base64String);

        // Assert
        decryptedString.Should().Be(originalString);
    }

    [Test]
    public async Task EncryptAsync_WithNullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] value = null!;

        // Act
        var act = async () => { await _dataProtectionService.EncryptAsync(value); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task DecryptAsync_WithNullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] value = null!;

        // Act
        var act = async () => { await _dataProtectionService.DecryptAsync(value); };

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task DecryptFromBase64Async_WithInvalidBase64String_ShouldThrowDataProtectionException()
    {
        // Arrange
        const string invalidBase64 = "InvalidBase64";

        // Act
        var act = async () => { await _dataProtectionService.DecryptFromBase64Async(invalidBase64); };

        // Assert
        await act.Should().ThrowAsync<DataProtectionException>();
    }

    [Test]
    public async Task DecryptAsync_WithCorruptedData_ShouldThrowDataProtectionException()
    {
        // Arrange
        var originalData = "Test data"u8.ToArray();
        var encryptedData = await _dataProtectionService.EncryptAsync(originalData);

        // Corrupt the encrypted data
        encryptedData[0] = (byte)(encryptedData[0] + 1);

        // Act
        Func<Task> act = () => _dataProtectionService.DecryptAsync(encryptedData);

        // Assert
        await act.Should().ThrowAsync<DataProtectionException>()
            .WithMessage("Decryption failed.");
    }

    [Test]
    public async Task DecryptAsync_WithIncorrectEntropy_ShouldThrowDataProtectionException()
    {
        // Arrange
        var originalData = "Test data"u8.ToArray();
        var additionalEntropy = "Entropy"u8.ToArray();
        var incorrectEntropy = "IncorrectEntropy"u8.ToArray();
        var encryptedData = await _dataProtectionService.EncryptAsync(originalData, additionalEntropy);

        // Act
        Func<Task> act = () => _dataProtectionService.DecryptAsync(encryptedData, incorrectEntropy);

        // Assert
        await act.Should().ThrowAsync<DataProtectionException>()
            .WithMessage("Decryption failed.");
    }
}