using backend.super_chatbot.Entidades.Requests.Meta;
using backend.super_chatbot.Repositories;
using backend.super_chatbot.Services.ClientMeta;

namespace backend.super_chatbot.Services.WebHookHandlers
{
    public class ButtonWebhookHandler : IWebHookHandler
    {
        private List<string> _distributionList = ["5511954392987", "5521998921716"];
        private IClientMeta _clientMeta;
        private IClientRepository _clientRepository;

        public ButtonWebhookHandler(IClientMeta clientMeta,
                                  IClientRepository clientRepository)
        {
            _clientMeta = clientMeta;
            _clientRepository = clientRepository;
        }
        public async Task HandleIncomingMessage(MessagesRequest message)
        {
            var senderPhoneNumber = message.GetSenderPhoneNumber();
            var clickedPayload = message.GetButton().Payload;

            var client = await _clientRepository.GetByPhoneNumber(senderPhoneNumber);
            if (client != null)
            {
                foreach (var item in _distributionList)
                    await _clientMeta.SendMessage(new SendTextMessageRequest()
                    {
                        To = item,
                        Text = new Text()
                        {
                            Body = $"O número {senderPhoneNumber} clicou no botão {clickedPayload}"
                        }
                    }
                , client);

            }
        }

    }
}
