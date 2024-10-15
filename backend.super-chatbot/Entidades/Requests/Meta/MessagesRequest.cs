namespace backend.super_chatbot.Entidades.Requests.Meta;

public class MessagesRequest
{
    public string Messaging_product { get; set; }
    public Metadata Metadata { get; set; }
    public Contact[] Contacts { get; set; }
    public Message[] Messages { get; set; }
}
public class Metadata
{
    public string display_phone_number { get; set; }
    public string phone_number_id { get; set; }
}

public class Contact
{
    public Profile profile { get; set; }
    public string wa_id { get; set; }
}

public class Profile
{
    public string name { get; set; }
}

public class Message
{
    public string from { get; set; }
    public string id { get; set; }
    public string timestamp { get; set; }
    public Text text { get; set; }
    public string type { get; set; }
}

public class Text
{
    public string body { get; set; }
}
