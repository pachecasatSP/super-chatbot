using Serilog;
using System.Net;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = Log.ForContext<ErrorHandlingMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.Information(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Ocorreu um erro inesperado: {ex.Message + ex.StackTrace}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new
            {
                context.Response.StatusCode,
                Message = "Ocorreu um erro. Tente novamente mais tarde."
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
