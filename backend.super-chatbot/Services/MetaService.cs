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
                    var statuses = request.GetStatuses();
                    if (statuses != null)
                    {                                                
                        _logger.Information("statuses received {@statuses}", statuses);
                    }                     
                    return;
                }
                
                var handler = _serviceProvider.GetKeyedService<IWebHookHandler>(message.Type)
                    ?? throw new ArgumentException($"Tipo: {message.Type} não possui um handler.");
                
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

        public async Task<string> SendVerificationCodeMessage(Requests.SendMessageRequest request, int senderId)
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
            return verificationCode.ToString();
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
