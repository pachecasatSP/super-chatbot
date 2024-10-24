using backend.super_chatbot.Configuration;
using backend.super_chatbot.Entidades;
using backend.super_chatbot.Entidades.Requests.Meta;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace backend.super_chatbot.Services.ClientMeta
{
    public class ClientMeta : IClientMeta
    {
        private WABConfiguration _config;

        public ClientMeta(IOptions<WABConfiguration> options)
        {
            _config = options.Value;
        }

        public Task<Stream> DownloadMedia(string url)
        {
            throw new NotImplementedException();
        }

        public async Task<MediaResponse> GetMedia(string mediaId, Client client)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_config.BaseAddress);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", client.TokenOnMeta);

            var response = await httpClient.GetAsync($"/media/{mediaId}");
            var responseText = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<MediaResponse>(responseText)!;
        }

        public async Task<Stream> DownloadMedia(string url, Client client)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_config.BaseAddress);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", client.TokenOnMeta);

            var response = await httpClient.GetStreamAsync(url);
            return response;
        }

        public async Task<string> SendMessage<T>(T request, Client client) where T : SendMessageRequest
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_config.BaseAddress);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", client.TokenOnMeta);

            var response = await httpClient.PostAsync(string.Format(_config.MessagesEndpoint, client.MetaPhoneId), new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);

            return responseText;
        }
    }
}
