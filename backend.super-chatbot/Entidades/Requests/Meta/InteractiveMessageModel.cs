namespace backend.super_chatbot.Entidades.Requests.Meta
{
    public class InteractiveMessageModel
    {
        public string? Type { get; set; } = "button";
        public Header? Header { get; set; }
        public Body? Body { get; set; }
        public Footer? Footer { get; set; }
        public InteractiveAction? Action { get; set; }
    }
}




