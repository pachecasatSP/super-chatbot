using Azure.Core;
using backend.super_chatbot.Configuration;
using backend.super_chatbot.Entidades;
using backend.super_chatbot.Entidades.Requests.Meta;
using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;
using System.Text;

namespace backend.super_chatbot.Services.ClientMeta
{
    public class ClientMeta : IClientMeta
    {
        private WABConfiguration _config;
        private Serilog.ILogger _logger;

        public ClientMeta(IOptions<WABConfiguration> options)
        {
            _config = options.Value;
            _logger = Log.ForContext<ClientMeta>();
        }
       
        public async Task<MediaResponse> GetMedia(string mediaId, Client client)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_config.BaseAddress!);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.TokenOnMeta);

            var response = await httpClient.GetAsync($"/{mediaId}");
            var responseText = await response.Content.ReadAsStringAsync();

            _logger.Information("responseText {@responseText}", responseText);
            if (!response.IsSuccessStatusCode)
                return null;

            return JsonConvert.DeserializeObject<MediaResponse>(responseText)!;
        }

        public async Task<Stream> DownloadMedia(string url, Client client)
        {
            using var httpClient = new HttpClient();            
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.TokenOnMeta);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.42.0");
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStreamAsync();
            response.EnsureSuccessStatusCode();

            return result ;
        }

        public async Task<string> SendMessage<T>(T request, Client client) where T : SendMessageRequest
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_config.BaseAddress!);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.TokenOnMeta);

            var response = await httpClient.PostAsync(string.Format(_config.MessagesEndpoint!, client.MetaPhoneId), new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);

            return responseText;
        }

        public async Task<MetaAuthTokenResponse> ExchangeCodeByToken(string? code)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_config.BaseAddress!);

            var requestBody = new
            {
                client_id = _config.Client_id,
                client_secret = _config.Client_secret,
                grant_type = "authorization_code",
                code
            };

            var response = await httpClient.PostAsync(_config.ExchangeTokenEndpoint, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);

            var result = JsonConvert.DeserializeObject<MetaAuthTokenResponse>(responseText);

            return result;
        }

        public async Task<bool> RegisterAccount(Client client, string customerPhoneNumberId)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_config.BaseAddress!);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.TokenOnMeta);

            var requestBody = new
            {
                messaging_product = "whatsapp",
                pin = new Random().Next(100000, 999999)
            };

            var response = await httpClient.PostAsync($"{customerPhoneNumberId}/register", new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);

            return true;
        }
    }
}
