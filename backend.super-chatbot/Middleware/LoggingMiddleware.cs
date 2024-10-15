using System.Diagnostics;

namespace backend.super_chatbot.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;        
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        await _next(context);

        stopwatch.Stop();
        var logDetails = new
        {
            RequestMethod = context.Request.Method,
            RequestPath = context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds
        };

       //_logger.Information("Request handled: {@LogDetails}", logDetails);
    }
}