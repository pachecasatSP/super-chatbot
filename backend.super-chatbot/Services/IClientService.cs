using backend.super_chatbot.Entidades.Requests;

namespace backend.super_chatbot.Services
{
    public interface IClientService
    {
        Task<CreateClientResponse> CreateClient(CreateClientRequest request);
        Task<CreateClientResponse> ExchangeToken(string authCode);
        Task RegisterAccount(int clientId, string phoneNumberId);
    }
}
