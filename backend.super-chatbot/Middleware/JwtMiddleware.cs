using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace backend.super_chatbot.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private List<string> publicEndpoints = ["HTTP: POST /client", "HTTP: GET /webhook", "HTTP: POST /webhook"];

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {        
        try
        {
            if (publicEndpoints.Contains(context.GetEndpoint()?.DisplayName!))
                await _next(context);

            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (token is null)
                throw new Exception();

            await AttachUserToContext(context, token!);

            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.CompleteAsync();
        }
    }

    private async Task AttachUserToContext(HttpContext context, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var openToken = tokenHandler.ReadJwtToken(token);

        if (DateTime.Now > openToken.ValidTo)
            throw new Exception("Token nao e valido");

        var name = openToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new ArgumentException("Token inválido.");
        var telNumber  = openToken.Claims.First(x => x.Type == ClaimTypes.MobilePhone)?.Value ?? throw new ArgumentException("Token inválido.");
        var metaPhoneId = openToken.Claims.First(x => x.Type == "metaphoneid")?.Value ?? throw new ArgumentException("Token inválido.");
        var clientId = openToken.Claims.First(x => x.Type == "clientId")?.Value ?? throw new ArgumentException("Token inválido.");

        context.Items["userName"] = name;
        context.Items["telNumber"] = telNumber;
        context.Items["metaPhoneId"] = metaPhoneId;
        context.Items["clientId"] = clientId;
    }

}
