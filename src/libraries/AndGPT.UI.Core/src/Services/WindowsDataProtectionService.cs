using System.Text;
using System.Security.Cryptography;
using AndGPT.Core.Contracts.Services;
using AndGPT.Core.Exceptions;

namespace AndGPT.UI.Core.Services;

public sealed class WindowsDataProtectionService : IDataProtectionService
{
    private readonly DataProtectionScope _scope = DataProtectionScope.CurrentUser;

    public Task<byte[]> EncryptAsync(byte[] value, byte[]? additionalEntropy = null)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        try
        {
            var encryptedData = ProtectedData.Protect(value, additionalEntropy, _scope);
            return Task.FromResult(encryptedData);
        }
        catch (CryptographicException ex)
        {
            throw new DataProtectionException("Encryption failed.", ex);
        }
    }

    public Task<byte[]> DecryptAsync(byte[] value, byte[]? additionalEntropy = null)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        try
        {
            var decryptedData = ProtectedData.Unprotect(value, additionalEntropy, _scope);
            return Task.FromResult(decryptedData);
        }
        catch (CryptographicException ex)
        {
            throw new DataProtectionException("Decryption failed.", ex);
        }
    }

    public async Task<string> EncryptToBase64Async(byte[] value, byte[]? additionalEntropy = null)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        try
        {
            var encryptedBytes = await EncryptAsync(value, additionalEntropy);
            return Convert.ToBase64String(encryptedBytes);
        }
        catch (DataProtectionException ex)
        {
            throw new DataProtectionException("Base64 encryption failed.", ex);
        }
    }

    public async Task<byte[]> DecryptFromBase64Async(string value, byte[]? additionalEntropy = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));

        try
        {
            var encryptedBytes = Convert.FromBase64String(value);
            return await DecryptAsync(encryptedBytes, additionalEntropy);
        }
        catch (FormatException ex)
        {
            throw new DataProtectionException("Invalid Base64 format.", ex);
        }
    }

    public async Task<string> EncryptStringToBase64Async(string value, byte[]? additionalEntropy = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));

        try
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return await EncryptToBase64Async(valueBytes, additionalEntropy);
        }
        catch (DataProtectionException ex)
        {
            throw new DataProtectionException("String to Base64 encryption failed.", ex);
        }
    }

    public async Task<string> DecryptStringFromBase64Async(string value, byte[]? additionalEntropy = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));

        try
        {
            var encryptedBytes = Convert.FromBase64String(value);
            var decryptedBytes = await DecryptAsync(encryptedBytes, additionalEntropy);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        catch (FormatException ex)
        {
            throw new DataProtectionException("Invalid Base64 format.", ex);
        }
    }
}