using Azure;
using backend.super_chatbot.Entidades.Requests;
using backend.super_chatbot.Entidades.Requests.Meta;
using backend.super_chatbot.Results;
using backend.super_chatbot.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.super_chatbot.Apis
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/customer", async ([FromServices] ILogger<Program> logger, [FromServices] IClientService service, CreateClientRequest request) =>
            {

                var response = await service.CreateClient(request);
                return response is CreateClientResponse
                    ? DefaultResults.CreateOkResultResponse(response)
                    : DefaultResults.CreateInvalidResult();

            }).WithName("create-client")
            .WithOpenApi();

            routes.MapPost("/customer/exchangetoken", async (IClientService service, OnboardingRequest request) =>
            {

                var response = await service.ExchangeToken(request.Code!);
                return response is CreateClientResponse
                    ? DefaultResults.CreateOkResultResponse(response)
                    : DefaultResults.CreateInvalidResult();

            }).WithName("exchange-token")
              .WithOpenApi();

            routes.MapPost("/customer/{phoneNumberId}/register", async (IClientService service, HttpContext context, string phoneNumberId) =>
            {

                try
                {
                    await service.RegisterAccount(int.Parse(context.Items["clientId"]?.ToString()!), phoneNumberId);
                    return DefaultResults.CreateOkResult();
                }
                catch (Exception ex) {
                    return DefaultResults.CreateInvalidResult(ex.Message);
                }
                
            }).WithName("register-account")
             .WithOpenApi();
        }
    }
}
