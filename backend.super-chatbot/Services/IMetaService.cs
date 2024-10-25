using backend.super_chatbot.Entidades;
using backend.super_chatbot.Entidades.Requests.Meta;
using Requests = backend.super_chatbot.Entidades.Requests;

namespace backend.super_chatbot.Services
{
    public interface IMetaService
    {
        Task<(string responseText, Client client)> SendMessage<T>(T request, int senderId) where T : SendMessageRequest;
        Task HandleWebhookMessage(MessagesRequest request);
        Task SendVerificationCodeMessage(Requests.SendMessageRequest  request, int senderId);
    }
}
