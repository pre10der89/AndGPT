namespace AndGPT.Core.Exceptions;

public sealed class DataProtectionException : Exception
{
    public DataProtectionException()
    {
    }

    public DataProtectionException(string message)
        : base(message)
    {
    }

    public DataProtectionException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
