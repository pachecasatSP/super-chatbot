using backend.super_chatbot.Configuration;
using backend.super_chatbot.Entidades;
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

        public async Task RedirectMessage(MessagesRequest request)
        {
            var client = await _clientRepository.GetByPhoneNumber(request.entry[0].changes[0].value.metadata.display_phone_number);
            if (client != null)
            {
                await SendMessage(new Entidades.Requests.Meta.SendMessageRequest()
                {
                    To = "5511954392987",
                    Text = new Text()
                    {
                        body = request.entry[0].changes[0].value.messages[0].text.body
                    }
                }
                , client.TokenMeta, client.Id_Telefone_Meta);

                await SendMessage(new Entidades.Requests.Meta.SendMessageRequest()
                {
                    To = "5521998921716",
                    Text = new Text()
                    {
                        body = request.entry[0].changes[0].value.messages[0].text.body
                    }
                }
               , client.TokenMeta, client.Id_Telefone_Meta);
            }
        }

        public async Task SendMessage(Entidades.Requests.Meta.SendMessageRequest request, int senderId)
        {
            var client = await _clientRepository.Get(senderId);

            if (client is null)
                throw new ArgumentException("Cliente inválido.");

            await SendMessage(request, client.TokenMeta, client.Id_Telefone_Meta);
        }

        private async Task SendMessage(Entidades.Requests.Meta.SendMessageRequest request, string tokenMeta, string idTelefoneMeta)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_config.BaseAddress);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenMeta);

            var response = await httpClient.PostAsJsonAsync(string.Format(_config.MessagesEndpoint, idTelefoneMeta), request);

            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);
        }
    }
}
