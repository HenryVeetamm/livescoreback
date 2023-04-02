using Azure;
using Exceptions;
using Newtonsoft.Json;

namespace API.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ResetColor();
            
            var errorResult = new ExceptionResult
            {
                Source = e.TargetSite?.DeclaringType?.FullName,
                Exception = e.Message.Trim(),
                StatusCode = 500
            };
            
            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = errorResult.StatusCode;
                await response.WriteAsync(JsonConvert.SerializeObject(errorResult));
            }
        }
    }
}