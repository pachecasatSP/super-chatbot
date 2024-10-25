using backend.super_chatbot.Configuration;
using backend.super_chatbot.Entidades;
using backend.super_chatbot.Entidades.Requests.Meta;
using backend.super_chatbot.Repositories;
using backend.super_chatbot.Services.ClientMeta;
using backend.super_chatbot.Services.WebHookHandlers;
using Newtonsoft.Json;
using Serilog;
using Requests = backend.super_chatbot.Entidades.Requests;

namespace backend.super_chatbot.Services
{
    public class MetaService : IMetaService
    {
        private WABConfiguration _config;
        private IClientRepository _clientRepository;
        private Serilog.ILogger _logger;
        private IServiceProvider _serviceProvider;
        private IClientMeta _clientMeta;
        private IContactRepository _contactRepository;

        public MetaService(IClientMeta clientMeta,
                              IClientRepository clientRepository,
                              IContactRepository contactRepository,
                              IServiceProvider serviceProvider)
        {

            _clientRepository = clientRepository;
            _contactRepository = contactRepository;
            _logger = Log.ForContext<MetaService>();
            _serviceProvider = serviceProvider;
            _clientMeta = clientMeta;
        }

        public async Task HandleWebhookMessage(MessagesRequest request)
        {
            if (request.Entry[0].Changes[0].Field == "messages")
            {
                var message = request.GetMessage();
                if (message is null)
                {
                    _logger.Information("null webhook message handled {@request}", request);
                    return;
                }

                var handler = _serviceProvider.GetKeyedService<IWebHookHandler>(message.Type)
                    ?? throw new ArgumentException($"Tipo: {message.Type} não possui um handler.");

                var senderPhoneNumber = request.GetSenderPhoneNumber();
                await MarkMessageReadAsync(message.Id!, senderPhoneNumber);
                await handler.HandleIncomingMessage(request);
            }
        }
        public async Task<(string responseText, Client client)> SendMessage<T>(T request, int senderId) where T : SendMessageRequest
        {
            var client = await _clientRepository.Get(senderId);

            if (client is null)
                throw new ArgumentException("Cliente inválido.");

            return (await _clientMeta.SendMessage(request, client), client);
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
                MetaMessageId = responseObject?.messages[0].Id!,
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

        private async Task MarkMessageReadAsync(string messageId, string senderPhoneNumber)
        {
            var client = await _clientRepository.GetByPhoneNumber(senderPhoneNumber);

            var request = new SetAsReadRequest()
            {
                Message_id = messageId
            };

            await _clientMeta.SendMessage(request, client);
        }
    }
}
