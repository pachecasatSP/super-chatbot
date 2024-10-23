namespace backend.super_chatbot.Entidades.Requests.Meta
{
    public class MessageResponse
    {
        public string messaging_product { get; set; }
        public Contact[] contacts { get; set; }
        public Message[] messages { get; set; }
    }

}

