namespace backend.super_chatbot.Entidades.Requests.Meta
{
    public class SendMessageRequest
    {
        public string Messaging_product { get { return "whatsapp"; } }
        public string To { get; set; }
        public Text Text { get; set; }
        public string Type { get { return "text"; } }
        public string recipient_type { get { return "individual"; } }

        public Context Context { get; set; }
    }

    public class Context
    {
        public string Message_id { get; set; }
    }
}

