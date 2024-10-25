﻿using backend.super_chatbot.Configuration;
using backend.super_chatbot.Entidades;
using backend.super_chatbot.Entidades.Requests.Meta;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
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

        public Task<Stream> DownloadMedia(string url)
        {
            throw new NotImplementedException();
        }

        public async Task<MediaResponse> GetMedia(string mediaId, Client client)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_config.BaseAddress);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", client.TokenOnMeta);

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
