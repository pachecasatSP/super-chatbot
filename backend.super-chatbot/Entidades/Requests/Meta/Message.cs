namespace backend.super_chatbot.Entidades.Requests.Meta;

public class Message
{
    public string? From { get; set; }
    public string? Id { get; set; }
    public string? Timestamp { get; set; }
    public string? Type { get; set; }
    public string? Message_status { get; set; }

    public Text? Text { get; set; }

    public Button Button { get; set; }

    public Document Document { get; set; }

    public InteractiveResponse Interactive { get; set; }
}
