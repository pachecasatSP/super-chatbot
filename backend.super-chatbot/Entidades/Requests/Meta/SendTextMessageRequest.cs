namespace backend.super_chatbot.Entidades.Requests.Meta
{
    public class SendTextMessageRequest : SendMessageRequest
    {
        public string Type { get { return "text"; } }
        public Text? Text { get; set; }
    }

}




