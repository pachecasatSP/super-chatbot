using backend.super_chatbot.Entidades.Requests.Meta;

namespace backend.super_chatbot.Services.WebHookHandlers
{
    public interface IWebHookHandler
    {
        Task HandleIncomingMessage(MessagesRequest message);
    }
}
