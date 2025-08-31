using Azure.Core;
using backend.super_chatbot.Entidades;
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
            var destPhoneNumber = message.GetDestinationPhoneNumber();
            var client = await _clientRepository.GetByPhoneNumber(destPhoneNumber);
            if (client != null)
            {
                var from = message.GetFrom();

                if (string.IsNullOrEmpty(from))
                    return;

                var requestFactory = RequestFactory.Create()
                                                   .SetClient(client)
                                                   .SetRecipient(message.GetContact().Profile.Name, from);
    
                var flow = client.Flow;
                foreach (var step in flow.Steps!)
                {
                    await SaveChat(message, client);

                    var request = requestFactory.HandleStep(step);
                    await _clientMeta.SendMessage(request, client);

                    if (step.WaitWebhook)
                        break;
                }
            }
        }

        private async Task SaveChat(MessagesRequest message, Client client)
        {
            var chat = new Chat()
            {
                CreatedDate = DateTime.Now,
                MetaMessageId = message.GetMessage()?.Id!
            };

            var contact = client.Contacts.FirstOrDefault(x => x.PhoneNumber == message.GetFrom());

            if (contact is null)
            {
                contact = new Entidades.Contact()
                {
                    PhoneNumber = message.GetFrom(),
                    Name = message.GetContact().Profile.Name
                };

                client.SetContact(contact);
            }
            contact.SetChat(chat);
            await _clientRepository.Save(client);
        }
    }
}
