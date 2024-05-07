using System.Text.RegularExpressions;

namespace HeyGPT.Core.Models;

public sealed record OpenAISecretKey
{
    public string Value { get; }

    public string ObfuscateValue { get; }

    public static OpenAISecretKey Empty => new();

    public bool IsEmpty => string.IsNullOrEmpty(Value);

    public OpenAISecretKey()
    {
        Value = string.Empty;
        ObfuscateValue = string.Empty;
    }

    public OpenAISecretKey(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "The numeric token id cannot be null");
        }

        // Strip all whitespace
        Value = value.Trim();
        ObfuscateValue = ObfuscateString(Value);
    }

    /// <summary>
    /// Extract the string representation of the specified <see cref="OpenAISecretKey"/>.
    /// </summary>
    /// <param name="value">The <see cref="OpenAISecretKey"/> object from which the string representation will be extracted.</param>
    public static implicit operator string(OpenAISecretKey value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value.Value;
    }

    /// <summary>
    /// Construct a <see cref="OpenAISecretKey"/> from the specified string value.
    /// </summary>
    /// <param name="value">The string value that represents a zipcode.</param>
    public static explicit operator OpenAISecretKey(string value) => new(value);

    public override string ToString() => ObfuscateValue;

    public static string ObfuscateString(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        // Prefix to check and preserve
        const string prefix = "sk-proj-";

        // Check if the input starts with the specified prefix
        if (input.StartsWith(prefix))
        {
            // Extract the part of the string after the prefix
            var postPrefix = input.Substring(prefix.Length);

            // Determine how many characters to obfuscate
            var preserveLength = Math.Min(6, postPrefix.Length);
            var obfuscateLength = postPrefix.Length - preserveLength;

            // Create the obfuscated part with asterisks
            var obfuscatedPart = new string('*', obfuscateLength);

            // Combine prefix, obfuscated part, and preserved part
            return prefix + obfuscatedPart + postPrefix.Substring(obfuscateLength);
        }
        else
        {
            // Handle strings without the prefix, preserving only the last 6 characters
            var preserveLength = Math.Min(6, input.Length);
            var obfuscateLength = input.Length - preserveLength;

            // Create the obfuscated part with asterisks
            var obfuscatedPart = new string('*', obfuscateLength);

            // Combine obfuscated part and preserved part
            return obfuscatedPart + input.Substring(obfuscateLength);
        }
    }
}