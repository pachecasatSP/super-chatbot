using backend.super_chatbot.Entidades;
using backend.super_chatbot.Entidades.Requests.Meta;

namespace backend.super_chatbot.Services.ClientMeta
{
    public interface IClientMeta
    {
        Task<string> SendMessage<T>(T request, Client client) where T : SendMessageRequest;
        Task<MediaResponse> GetMedia(string mediaId, Client client);
        Task<Stream> DownloadMedia(string url, Client client);
    }
}
