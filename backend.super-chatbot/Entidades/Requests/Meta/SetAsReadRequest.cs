namespace backend.super_chatbot.Entidades.Requests.Meta
{
    public class SetAsReadRequest : SendMessageRequest
    {
        public string? Messaging_product { get { return "whatsapp"; } }
        public string? Status { get { return "read"; } }
        public string? Message_id { get; set; }
    }
}
