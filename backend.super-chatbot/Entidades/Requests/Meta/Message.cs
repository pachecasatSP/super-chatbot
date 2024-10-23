namespace backend.super_chatbot.Entidades.Requests.Meta;

public class Message
{
    public string from { get; set; }
    public string id { get; set; }
    public string timestamp { get; set; }
    public Text text { get; set; }

    public Button button { get; set; }
    public string type { get; set; }
    public string message_status { get; set; }
}
