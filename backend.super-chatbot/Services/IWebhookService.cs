using Meta = backend.super_chatbot.Entidades.Requests.Meta;

namespace backend.super_chatbot.Services
{
    public interface IWebhookService
    {
        Task RedirectMessage(Meta.MessagesRequest request);
        Task SendMessage(Meta.SendMessageRequest request, int senderId);
    }
}
