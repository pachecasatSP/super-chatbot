using backend.super_chatbot.Entidades.Requests;

namespace backend.super_chatbot.Services
{
    public interface IClientService
    {
        Task<CreateClientResponse> CreateClient(CreateClientRequest request);
    }
}
