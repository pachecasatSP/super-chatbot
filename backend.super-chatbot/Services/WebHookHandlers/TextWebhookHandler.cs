using backend.super_chatbot.Entidades.Requests.Meta;
using backend.super_chatbot.Repositories;
using backend.super_chatbot.Services.ClientMeta;

namespace backend.super_chatbot.Services.WebHookHandlers
{
    public class TextWebhookHandler : IWebHookHandler
    {

        private IClientMeta _clientMeta;
        private IClientRepository _clientRepository;

        public TextWebhookHandler(IClientMeta clientMeta,
                                  IClientRepository clientRepository)
        {
            _clientMeta = clientMeta;
            _clientRepository = clientRepository;
        }
        public async Task HandleIncomingMessage(MessagesRequest message)
        {
            var senderPhoneNumber = message.GetSenderPhoneNumber();            
            var client = await _clientRepository.GetByPhoneNumber(senderPhoneNumber);
            if (client != null)
            {

                var from = message.GetFrom();

                if (string.IsNullOrEmpty(from))
                    return;

                await _clientMeta.SendMessage(new SendIteractiveMessageRequest()
                {
                    To = from,
                     Iteractive = new InteractiveMessageModel()
                     {
                        Body = new Body() { Text = $"Olá {message.GetContact().Profile.Name}. Seja bem-vindo a nossa ferramenta!\r\nAbaixo você encontra as opções disponíveis." },
                        Footer = new Footer() { Text = $"Selecione a opção desejada."},
                        Action = new InteractiveAction()
                        {
                            Buttons = [new ActionButton() {  Reply = new Reply(){Id = "download-template",Title = "Baixar Template"} },
                                       new ActionButton() {  Reply = new Reply(){Id = "upload-sheet",Title = "Enviar planilha"} }]
                        }
                     }
                }
            , client);

            }
        }
    }
}
