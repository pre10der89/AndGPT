namespace HeyGPT.Core.Models;

public readonly record struct CharacterType
{
    public string Value
    {
        get;
    }

    public static CharacterType Default => new("Default");

    public static CharacterType Local = new("Local");

    public static CharacterType Error = new("Error");

    public bool IsSpecified => !string.IsNullOrEmpty(Value);

    public CharacterType()
    {
        Value = string.Empty;
    }

    public CharacterType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "The value cannot be null");
        }

        // Strip all whitespace
        Value = value.ToLowerInvariant().Trim();
    }


    /// <summary>
    /// Extract the string representation of the specified <see cref="CharacterType"/>.
    /// </summary>
    /// <param name="value">The <see cref="CharacterType"/> object from which the string representation will be extracted.</param>
    public static implicit operator string(CharacterType value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value.Value;
    }

    /// <summary>
    /// Construct a <see cref="CharacterType"/> from the specified string value.
    /// </summary>
    /// <param name="value">The string value that represents the object.</param>
    public static explicit operator CharacterType(string value) => new(value);

    public override int GetHashCode()
    {
        return Value?.ToLowerInvariant().GetHashCode() ?? 0;
    }

    public override string ToString() => Value;
}
