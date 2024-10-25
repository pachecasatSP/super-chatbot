using backend.super_chatbot.Entidades;
using backend.super_chatbot.Entidades.Requests.Meta;
using backend.super_chatbot.Repositories;
using backend.super_chatbot.Services.ClientMeta;

namespace backend.super_chatbot.Services.WebHookHandlers
{
    public class InteractiveWebhookHandler : IWebHookHandler
    {
        private IClientMeta _clientMeta;
        private IClientRepository _clientRepository;

        public InteractiveWebhookHandler(IClientMeta clientMeta,
                                         IClientRepository clientRepository)
        {
            _clientMeta = clientMeta;
            _clientRepository = clientRepository;
        }

        public async Task HandleIncomingMessage(MessagesRequest message)
        {
            var payload = message.GetInteractiveResponse();
            if (payload is null)
                return;

            var client = await _clientRepository.GetByPhoneNumber(message.GetSenderPhoneNumber());
            switch (payload.Button_reply.Id)
            {
                case "download-template":
                    await HandleDownloadTemplateButton(message, client);
                    break;
                case "upload-sheet":
                    await HandleUploadSheetButton(message, client);
                    break;
                default:
                    break;

            }

        }

        private async Task HandleUploadSheetButton(MessagesRequest message, Client client)
        {            
            await _clientMeta.SendMessage(new SendTextMessageRequest()
            {
                To = message.GetFrom(),
                Text = new Text()
                {
                    Body = $"Certo, {message.GetContact().Profile.Name}. Pode enviar o arquivo. \n\r Você será notificado quando finalizarmos o processamento."
                }
            }, client);
        }

        private async Task HandleDownloadTemplateButton(MessagesRequest message, Client client)
        {
            await _clientMeta.SendMessage(new SendDocumentMessageRequest()
            {
                To = message.GetFrom(),
                Document = new DocumentDetails()
                {
                     Id = "2813583085488085",
                     Caption = $"Prontinho {message.GetContact().Profile.Name}, \n\raqui está o template de arquivo que precisa ser preenchido."
                }
            }, client);
        }
    }
}
