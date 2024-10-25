namespace backend.super_chatbot.Entidades.Requests.Meta
{
    public class SendIteractiveMessageRequest : SendMessageRequest
    {
        public string Type { get { return "interactive"; } }
        public InteractiveMessageModel? Interactive { get; set; }
    }
}




