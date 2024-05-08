namespace HeyGPT.Core.Models;

public readonly record struct CommunityRole
{
    public string Value
    {
        get;
    }

    public static CommunityRole Empty => new();

    public bool IsEmpty => string.IsNullOrEmpty(Value);

    public CommunityRole()
    {
        Value = string.Empty;
    }

    public CommunityRole(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "The value cannot be null");
        }

        // Strip all whitespace
        Value = value.ToLowerInvariant().Trim();
    }


    /// <summary>
    /// Extract the string representation of the specified <see cref="CommunityRole"/>.
    /// </summary>
    /// <param name="value">The <see cref="CommunityRole"/> object from which the string representation will be extracted.</param>
    public static implicit operator string(CommunityRole value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value.Value;
    }

    /// <summary>
    /// Construct a <see cref="CommunityRole"/> from the specified string value.
    /// </summary>
    /// <param name="value">The string value that represents the object.</param>
    public static explicit operator CommunityRole(string value) => new(value);

    public override int GetHashCode()
    {
        // Use case-insensitive hash code for the Message property
        return Value?.ToLowerInvariant().GetHashCode() ?? 0;
    }

    public override string ToString() => Value;
}
