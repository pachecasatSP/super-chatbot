using backend.super_chatbot.Configuration;
using backend.super_chatbot.Entidades.Requests.Meta;
using backend.super_chatbot.Repositories;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Requests = backend.super_chatbot.Entidades.Requests;
using System.Text;
using backend.super_chatbot.Entidades;

namespace backend.super_chatbot.Services
{
    public class MetaService : IMetaService
    {
        private WABConfiguration _config;
        private IClientRepository _clientRepository;
        private ILogger<MetaService> _logger;
        private IContactRepository _contactRepository;

        public MetaService(IOptions<WABConfiguration> options,
                              IClientRepository clientRepository,
                              IContactRepository contactRepository,
                              ILogger<MetaService> logger)
        {
            _config = options.Value;
            _clientRepository = clientRepository;
            _logger = logger;
            _contactRepository = contactRepository;
        }

        public async Task HandleMessage(MessagesRequest request)
        {
            if (request.entry[0].changes[0].field == "messages")
            {
                var message = request.entry[0].changes[0].value.messages[0];
                if (message.type == "text")
                    await RedirectMessage(request.entry[0].changes[0].value.metadata.display_phone_number, message.text.body);
                else if (message.type == "button")
                    await HandleButtonClick(message);
            }
        }

        private async Task HandleButtonClick(Message message)
        {
            throw new NotImplementedException();
        }

        private async Task RedirectMessage(string senderPhoneNumber, string message)
        {
            var client = await _clientRepository.GetByPhoneNumber(senderPhoneNumber);
            if (client != null)
            {
                await SendMessage(new SendTextMessageRequest()
                {
                    To = "5511954392987",
                    Text = new Text()
                    {
                        body = message
                    }
                }
                , client.TokenOnMeta, client.MetaPhoneId);

                await SendMessage(new SendTextMessageRequest()
                {
                    To = "5521998921716",
                    Text = new Text()
                    {
                        body = message
                    }
                }
               , client.TokenOnMeta, client.MetaPhoneId);
            }
        }
        public async Task<(string responseText, Client client)> SendMessage<T>(T request, int senderId) where T : SendMessageRequest
        {
            var client = await _clientRepository.Get(senderId);

            if (client is null)
                throw new ArgumentException("Cliente inválido.");

            return (await SendMessage(request, client.TokenOnMeta, client.MetaPhoneId), client);
        }

        private async Task<string> SendMessage<T>(T request, string tokenMeta, string idTelefoneMeta) where T : SendMessageRequest
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_config.BaseAddress);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenMeta);


            var response = await httpClient.PostAsync(string.Format(_config.MessagesEndpoint, idTelefoneMeta), new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseText);

            return responseText;
        }

        public async Task SendVerificationCodeMessage(Requests.SendMessageRequest request, int senderId)
        {
            var verificationCode = new Random().Next(100000, 999999);

            var sendVerificationCodeRequest = new SendTemplateMessageRequest()
            {
                To = request.NumeroDestino,
                Template = new Template()
                {
                    Components = [new BodyComponent() {
                            Type = "body",
                            Parameters = [new TextParameter() { Text = verificationCode.ToString() }]
                    },
                    new ButtonComponent(){
                          Index = "0",
                          Sub_type = "url",
                          Parameters = [new ButtonParameters(){ Type="text" ,Text=verificationCode.ToString()}]
                    }],
                    Name = "codigo_validacao"
                }
            };

            var response = await SendMessage(sendVerificationCodeRequest, senderId);
            var responseObject = JsonConvert.DeserializeObject<MessageResponse>(response.responseText);

            var client = response.client;

            var chat = new Chat()
            {
                ContactId = senderId,
                VerificationCode = verificationCode.ToString(),
                VerificationCodeExpiration = DateTime.Now.AddMinutes(90),
                MetaMessageId = responseObject?.messages[0].id!,
                CreatedDate = DateTime.Now
            };

            var contact = await _contactRepository.GetByPhoneNumber(request.NumeroDestino);

            contact ??= new Entidades.Contact()
            {
                PhoneNumber = request.NumeroDestino,
                ClientId = senderId,
                Name = request.NumeroDestino
            };

            contact.SetChat(chat);

            client.SetContact(contact);
            await _clientRepository.Save(client);
        }
    }
}
