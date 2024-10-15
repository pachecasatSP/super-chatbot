using backend.super_chatbot.Entidades.Requests.Meta;
using backend.super_chatbot.Results;
using backend.super_chatbot.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace backend.super_chatbot.Apis
{
    public static class WebHookEndpoints
    {
        public static void MapWebhookEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/webhook", async ([FromServices] ILogger<Program> logger, HttpContext context) =>
            {
                var mode = context.Request.Query["hub.mode"];
                var verifyToken = context.Request.Query["hub.verify_token"];
                var challenge = context.Request.Query["hub.challenge"];

                return (mode == "subscribe" && verifyToken == "cfkHGyVupvdOnU89NIxovLCCp")
                    ? DefaultResults.CreateOkResult(challenge!)
                    : DefaultResults.CreateInvalidResult();
            })
            .WithName("SubscribeEndpoint");

            routes.MapPost("/webhook", async ([FromServices] ILogger<Program> logger, [FromServices] IWebhookService service, MessagesRequest request) =>
            {
                await service.RedirectMessage(request);                                    
            })            
            .WithName("WebHookInvoke");
        }
    }
}
