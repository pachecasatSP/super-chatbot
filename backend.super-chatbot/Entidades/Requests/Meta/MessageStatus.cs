namespace backend.super_chatbot.Entidades.Requests.Meta
{
    public class MessageStatus
    {
        public string? Id { get; set; }
        public string? Status { get; set; }
        public string? Timestamp { get; set; }
        public string? Recipient_id { get; set; }
        public Conversation? Conversation { get; set; }
        public Pricing? Pricing { get; set; }
    }

    public class Conversation
    {
        public string? Id { get; set; }
        public Origin? Origin { get; set; }
    }

    public class Origin
    {
        public string? Id { get; set; }
    }

    public class Pricing
    {
        public bool Billable { get; set; }
        public string? Pricing_model { get; set; }
        public string? Category { get; set; }

    }
}


