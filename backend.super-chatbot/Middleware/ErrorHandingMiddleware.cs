using System.Net;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogInformation(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ocorreu um erro inesperado: {ex.Message + ex.StackTrace}");

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
