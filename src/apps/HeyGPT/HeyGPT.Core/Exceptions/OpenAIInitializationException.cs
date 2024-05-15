namespace HeyGPT.Core.Exceptions;

public class OpenAIInitializationException : Exception
{
    public OpenAIInitializationException()
    {
    }

    public OpenAIInitializationException(string message)
        : base(message)
    {
    }

    public OpenAIInitializationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

