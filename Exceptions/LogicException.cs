namespace Exceptions;

public class LogicException : Exception
{
    public string ErrorText { get; set; }
    public LogicException(string message) : base(message)
    {
        ErrorText = message;
    }
}