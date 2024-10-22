using backend.super_chatbot.Entidades.Requests.Meta;
using backend.super_chatbot.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace backend.super_chatbot.Apis
{
    public static class WebHookEndpoints
    {
        private static string verifyToken = "cfkHGyVupvdOnU89NIxovLCCp";

        public static void MapWebhookEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/webhook", ReadHub);

            static string ReadHub([FromQuery(Name = "hub.mode")] string mode, [FromQuery(Name = "hub.verify_token")] string verify_token, [FromQuery(Name = "hub.challenge")] string challenge)
            {
                if (mode == "subscribe" && verify_token == verifyToken)
                    return challenge;

                return string.Empty;
            }

            routes.MapPost("/webhook", async ([FromServices] ILogger<Program> logger, [FromServices] IMetaService service, MessagesRequest request) =>
            {
                service.HandleMessage(request);
            })
            .WithName("WebHookInvoke");
        }
    }




}
