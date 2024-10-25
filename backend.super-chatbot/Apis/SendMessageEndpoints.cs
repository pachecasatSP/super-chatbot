using backend.super_chatbot.Entidades.Requests;
using backend.super_chatbot.Entidades.Requests.Meta;
using backend.super_chatbot.Results;
using backend.super_chatbot.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace backend.super_chatbot.Apis
{
    public static class SendMessageEndpoints
    {
        public static void MapSendMessageEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/sendtextmessage", async ([FromServices] ILogger<Program> logger,
                                                      [FromServices] IMetaService service,
                                                      HttpContext context, Entidades.Requests.SendTextMessageRequest request) =>
            {
                await service.SendMessage(new Entidades.Requests.Meta.SendTextMessageRequest()
                {
                    Text = new Text()
                    {
                        Body = request.Message
                    },
                    To = request.NumeroDestino
                }, int.Parse(context.Items["clientId"]?.ToString()!));


                return DefaultResults.CreateOkResult();
            });

            routes.MapPost("/sendoptinMessage", async ([FromServices] ILogger<Program> logger,
                                                       [FromServices] IMetaService service,
                                                       HttpContext context, Entidades.Requests.SendTemplateMessageRequest request) =>
            {
               
                await service.SendMessage(new Entidades.Requests.Meta.SendTemplateMessageRequest()
                {
                    Template = new Template()
                    {
                        Name = "opt_in",
                        Components = [new HeaderComponent() { Parameters = request.Parameters.Select(x => new TextParameter() { Text = x }).ToArray() }]
                    },
                    To = request.NumeroDestino
                }, int.Parse(context.Items["clientId"]?.ToString()!));


                return DefaultResults.CreateOkResult();
            });

            routes.MapPost("/sendverificationcode", async ([FromServices] ILogger<Program> logger,
                                                           [FromServices] IMetaService service,
                                                       HttpContext context, Entidades.Requests.SendMessageRequest request) =>
            {              

                await service.SendVerificationCodeMessage(request, int.Parse(context.Items["clientId"]?.ToString()!));


                return DefaultResults.CreateOkResult();
            });
        }
    }
}
