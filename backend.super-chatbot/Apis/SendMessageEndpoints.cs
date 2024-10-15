using backend.super_chatbot.Entidades.Requests;
using backend.super_chatbot.Results;
using backend.super_chatbot.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.super_chatbot.Apis
{
    public static class SendMessageEndpoints
    {
        public static void MapSendMessageEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/sendmessage", async ([FromServices] ILogger<Program> logger, [FromServices] IWebhookService service, HttpContext context, SendMessageRequest request) =>
            {
                await service.SendMessage(new Entidades.Requests.Meta.SendMessageRequest()
                {
                    Text = new Entidades.Requests.Meta.Text()
                    {
                        body = request.Message
                    },
                    To = request.NumeroDestino
                }, new Sender()
                {
                    Nome = context.Items["userName"]?.ToString(),
                    TelNumber = context.Items["telNumber"]?.ToString(),
                    TelId = context.Items["metaPhoneId"]?.ToString(),
                    Id = context.Items["clientId"]?.ToString(),
                });


                return DefaultResults.CreateOkResult();
            });
        }
    }
}
