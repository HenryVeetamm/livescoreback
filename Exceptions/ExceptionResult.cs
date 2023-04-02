namespace Exceptions;

public class ExceptionResult
{
    public List<string> Messages { get; set; } = new();

    public string? Source { get; set; }
    public string? Exception { get; set; }
    public string? SupportMessage { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
}