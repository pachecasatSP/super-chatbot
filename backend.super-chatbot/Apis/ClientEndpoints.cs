using backend.super_chatbot.Entidades.Requests;
using backend.super_chatbot.Results;
using backend.super_chatbot.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.super_chatbot.Apis
{
    public static class ClientEndpoints
    {
        public static void MapClientEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/client", async ([FromServices] ILogger<Program> logger, [FromServices] IClientService service, CreateClientRequest request) =>
            {

                var response = await service.CreateClient(request);
                return response is CreateClientResponse
                    ? DefaultResults.CreateOkResultResponse(response)
                    : DefaultResults.CreateInvalidResult();

            }).WithName("CreateClient")
              .WithOpenApi();
        }
    }
}
