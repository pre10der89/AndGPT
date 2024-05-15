namespace HeyGPT.Core.Exceptions;

public class OpenAISendCompletionException : Exception
{
    public OpenAISendCompletionException()
    {
    }

    public OpenAISendCompletionException(string message)
        : base(message)
    {
    }

    public OpenAISendCompletionException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

