using backend.super_chatbot.Entidades.Requests.Meta;

namespace backend.super_chatbot.Entidades
{
    public class Flow
    {
        public Client Client { get; set; }
        public ICollection<Step> Steps { get; set; }
    }

    public class Step
    {
        public string? Number { get; set; }
        public string? Type { get; set; }
        public Content? Content { get; set; }
        public int NextStep { get; set; }
        public bool WaitWebhook { get; set; }
    }

    public class Content
    {
        public string? Type { get; set; }
        public string? ContentValue { get; set; }
    }

    public class ButtonDefinition
    {
        public string Title { get; set; }
        public string Type { get; private set; }

        public ButtonType? Definition { get; set; }
    }

    public abstract class ButtonType
    {
        public ButtonType(string type)
        {
            Type = type;
        }
        public string Type { get; private set; }
    }

    public class ApiCallButtonType : ButtonType
    {
        public ApiCallButtonType()
            : base("apicall") { }

        public string? Method { get; set; }
        public string? RequestUri { get; set; }
        public string? JsonBody { get; set; }
        public Dictionary<string, string>? Headers { get; set; }
    }

    public class CallMenuButtonType : ButtonType
    {
        public CallMenuButtonType()
            : base("menucall") { }

        public string? MenuName { get; set; }
    }

    public class SendTextMesssageButtonType : ButtonType
    {
        public SendTextMesssageButtonType()
           : base("sendtextmessage") { }

        public string? TextMessage { get; set; }
    }

    public class MenuDefinition
    {
        public string? MenuName { get; set; }
        public Button[] Buttons { get; set; }
    }
}