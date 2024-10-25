namespace backend.super_chatbot.Entidades.Requests.Meta
{
    public class SendDocumentMessageRequest : SendMessageRequest
    {

        public string Type { get { return "document"; } }

        public DocumentDetails? Document { get; set; }
        
    }

}




