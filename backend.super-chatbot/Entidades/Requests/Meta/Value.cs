namespace backend.super_chatbot.Entidades.Requests.Meta;

public class Value
{
    public string Messaging_product { get; set; }
    public Metadata Metadata { get; set; }
    public Contact[] Contacts { get; set; }
    public Message[] Messages { get; set; }
}
