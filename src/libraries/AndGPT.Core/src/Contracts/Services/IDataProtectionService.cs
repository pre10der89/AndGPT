using AndGPT.Core.Exceptions;

namespace AndGPT.Core.Contracts.Services;

/// <summary>
/// Defines methods for data protection, including encryption and decryption of byte arrays, strings, and Base64 encoded data.
/// </summary>
public interface IDataProtectionService
{
    /// <summary>
    /// Encrypts the specified byte array asynchronously.
    /// </summary>
    /// <param name="value">The byte array to encrypt.</param>
    /// <param name="additionalEntropy">An optional byte array used to add entropy to the encryption process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the encrypted byte array.</returns>
    /// <exception cref="DataProtectionException">Thrown when encryption fails.</exception>
    Task<byte[]> EncryptAsync(byte[] value, byte[]? additionalEntropy = null);

    /// <summary>
    /// Decrypts the specified byte array asynchronously.
    /// </summary>
    /// <param name="value">The byte array to decrypt.</param>
    /// <param name="additionalEntropy">An optional byte array used to add entropy to the decryption process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the decrypted byte array.</returns>
    /// <exception cref="DataProtectionException">Thrown when decryption fails.</exception>
    Task<byte[]> DecryptAsync(byte[] value, byte[]? additionalEntropy = null);

    /// <summary>
    /// Encrypts the specified byte array and returns the encrypted data as a Base64 string asynchronously.
    /// </summary>
    /// <param name="value">The byte array to encrypt.</param>
    /// <param name="additionalEntropy">An optional byte array used to add entropy to the encryption process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the encrypted data as a Base64 string.</returns>
    /// <exception cref="DataProtectionException">Thrown when encryption or Base64 encoding fails.</exception>
    Task<string> EncryptToBase64Async(byte[] value, byte[]? additionalEntropy = null);

    /// <summary>
    /// Decrypts the specified Base64 string and returns the decrypted data as a byte array asynchronously.
    /// </summary>
    /// <param name="value">The Base64 string to decrypt.</param>
    /// <param name="additionalEntropy">An optional byte array used to add entropy to the decryption process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the decrypted byte array.</returns>
    /// <exception cref="DataProtectionException">Thrown when decryption or Base64 decoding fails.</exception>
    Task<byte[]> DecryptFromBase64Async(string value, byte[]? additionalEntropy = null);

    /// <summary>
    /// Encrypts the specified string and returns the encrypted data as a Base64 string asynchronously.
    /// </summary>
    /// <param name="value">The string to encrypt.</param>
    /// <param name="additionalEntropy">An optional byte array used to add entropy to the encryption process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the encrypted data as a Base64 string.</returns>
    /// <exception cref="DataProtectionException">Thrown when encryption or Base64 encoding fails.</exception>
    Task<string> EncryptStringToBase64Async(string value, byte[]? additionalEntropy = null);

    /// <summary>
    /// Decrypts the specified Base64 string and returns the decrypted data as a string asynchronously.
    /// </summary>
    /// <param name="value">The Base64 string to decrypt.</param>
    /// <param name="additionalEntropy">An optional byte array used to add entropy to the decryption process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the decrypted string.</returns>
    /// <exception cref="DataProtectionException">Thrown when decryption or Base64 decoding fails.</exception>
    Task<string> DecryptStringFromBase64Async(string value, byte[]? additionalEntropy = null);
}