using backend.super_chatbot.Entidades.Requests.Meta;
using backend.super_chatbot.Repositories;
using backend.super_chatbot.Services.ClientMeta;
using Serilog;

namespace backend.super_chatbot.Services.WebHookHandlers
{
    public class DocumentWebhookHandler : IWebHookHandler
    {
        private IClientMeta _clientMeta;
        private IClientRepository _clientRepository;
        private Serilog.ILogger _logger;

        public DocumentWebhookHandler(IClientMeta clientMeta,
                                      IClientRepository clientRepository)
        {
            _clientMeta = clientMeta;
            _clientRepository = clientRepository;
            _logger = Log.ForContext<DocumentWebhookHandler>();
        }

        public async Task HandleIncomingMessage(MessagesRequest message)
        {
            var senderPhoneNumber = message.GetSenderPhoneNumber();
            var documentInfo = message.GetDocument();

            var client = await _clientRepository.GetByPhoneNumber(senderPhoneNumber);

            var document = await _clientMeta.GetMedia(documentInfo.Id!, client);

            if (document is null)
            {
                _logger.Information("DocumentInfo vazio");
                return;
            }

            _logger.Information("Document details {@documentDetails}", document);

            var result = await _clientMeta.DownloadMedia(document.Url!, client);

            if (document.Mime_type == "text/plain")
            {
                var resultText = ReadPlainText(result);
                _logger.Information("Received {@result}", result);
            }
        }

        private object ReadPlainText(Stream result)
        {
            using var sr = new StreamReader(result);
            return sr.ReadToEnd();
        }
    }
}
