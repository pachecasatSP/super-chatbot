﻿using backend.super_chatbot.Entidades.Requests.Meta;
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

            _logger.Information("Document details {@documentDetails}", document);

            var stream = await _clientMeta.DownloadMedia(document.Url!, client);

            using var sr = new StreamReader(stream);
            stream.Position = 0;
            var streamText =  await sr.ReadToEndAsync();
            _logger.Information("Received {@stream}", streamText);  

        }
    }
}
