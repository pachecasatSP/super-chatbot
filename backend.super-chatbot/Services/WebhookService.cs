using backend.super_chatbot.Configuration;
using backend.super_chatbot.Entidades.Requests;
using backend.super_chatbot.Entidades.Requests.Meta;
using backend.super_chatbot.Repositories;
using Microsoft.Extensions.Options;

namespace backend.super_chatbot.Services
{
    public class WebhookService : IWebhookService
    {
        private WABConfiguration _config;
        private IClientRepository _clientRepository;

        public WebhookService(IOptions<WABConfiguration> options,
                              IClientRepository clientRepository)
        {
            _config = options.Value;
            _clientRepository = clientRepository;
        }

        public Task RedirectMessage(MessagesRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task SendMessage(Entidades.Requests.Meta.SendMessageRequest request, Sender sender)
        {
            var client = await _clientRepository.Get(int.Parse(sender.Id!));

            if (client is null)
                throw new ArgumentException("Cliente inválido.");

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_config.BaseAddress);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", client.TokenMeta);

            var response = await httpClient.PostAsJsonAsync(string.Format(_config.MessagesEndpoint, client.Id_Telefone_Meta), request);

            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);
        }
    }
}
