namespace backend.super_chatbot.Entidades.Requests
{
    public class SendMessageRequest
    {
        public string NumeroDestino { get; set; }        
    }

    public class SendTextMessageRequest : SendMessageRequest
    {
        public string Message { get; set; }
    }

    public class SendTemplateMessageRequest : SendMessageRequest { 
        public string[] Parameters { get; set; }

    }
}
