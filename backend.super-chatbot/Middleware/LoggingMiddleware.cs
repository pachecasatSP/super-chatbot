using Serilog;
using Serilog.Context;
using System.Diagnostics;

namespace backend.super_chatbot.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
        _logger = Log.ForContext<LoggingMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Body.CanSeek)
            context.Request.EnableBuffering();

        context.Request.Body.Position = 0;
        var bodyText = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        using (LogContext.PushProperty("requestBody", bodyText))
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
                       
            await _next(context);

            stopwatch.Stop();
            var logDetails = new
            {
                Method = context.Request.Method,
                Path = context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                bodyText
            };

            _logger.Information("Request handled: {@logDetails}", logDetails);
        }
    }
}