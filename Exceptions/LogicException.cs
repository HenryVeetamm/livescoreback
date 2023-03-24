namespace Exceptions;

public class LogicException : Exception
{
    public LogicException(string message) : base(message)
    {
        ErrorText = message;
    }

    public LogicException(string message, Exception inner) : base(message, inner)
    {
        ErrorText = message;
    }

    public string ErrorText { get; set; }
}