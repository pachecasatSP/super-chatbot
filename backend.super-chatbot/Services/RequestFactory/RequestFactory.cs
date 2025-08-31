using backend.super_chatbot.Entidades;
using backend.super_chatbot.Entidades.Requests.Meta;
using Newtonsoft.Json;

namespace backend.super_chatbot.Services
{
    public class RequestFactory
    {
        private RequestFactory() { }

        private static RequestFactory? _instance;

        public static Client? Client { get; private set; }
        public string? RecipientName { get; private set; }
        public string RecipientNumber { get; private set; }

        public static RequestFactory Create()
        {
            if (_instance is null)
            {
                _instance = new RequestFactory();
            }
            return _instance;
        }
        public RequestFactory SetClient(Client client)
        {
            Client = client;
            return this;
        }

        public RequestFactory SetRecipient(string recipientName, string recipientNumber)
        {
            RecipientName = recipientName;
            RecipientNumber = recipientNumber;
            return this;
        }

        public SendMessageRequest HandleStep(Step step) => step.Type switch
        {
            "greeting" => CreateGreetingMessageRequest(step.Content?.ContentValue!),
            "menu" => CreateMenuMessageRequest(step.Content?.ContentValue!),
            _ => new SendMessageRequest()
        };

        private SendMessageRequest CreateMenuMessageRequest(string value)
        {
            var request = JsonConvert.DeserializeObject<SendIteractiveMessageRequest>(value)!;
            request.To = RecipientNumber;
            return request;
        }


        private SendTextMessageRequest CreateGreetingMessageRequest(string value)
        {
            var request = JsonConvert.DeserializeObject<SendTextMessageRequest>(value);
            var body = request?.Text?.Body?.Replace("{{ProfileName}}", RecipientName)
                             .Replace("{{ClientName}}", Client?.FriendlyName);

            request.To = RecipientNumber;
            request.Text.Body = body;
            return request!;
        }
    }
}
