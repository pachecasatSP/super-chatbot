using backend.super_chatbot.Entidades.Requests.Meta;
using Requests = backend.super_chatbot.Entidades.Requests;

namespace backend.super_chatbot.Services
{
    public interface IMetaService
    {
        Task<string> SendMessage<T>(T request, int senderId) where T : SendMessageRequest;
        Task HandleMessage(MessagesRequest request);
        Task SendVerificationCodeMessage(Requests.SendMessageRequest  request, int senderId);
    }
}
