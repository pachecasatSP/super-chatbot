namespace backend.super_chatbot.Entidades.Requests.Meta
{
    public class SendTemplateMessageRequest : SendMessageRequest
    {
        public string Type { get { return "template"; } }

        public Template Template { get; set; }

    }

    public class Template
    {

        public string Name { get; set; }

        public Language Language { get; set; } = new Language();

        public Component[] Components { get; set; }
       
    }

    public class Action
    {
        public string Button { get; set; }

        public ActionButton[] Buttons { get; set; }
    }

    public class ActionButton
    {
        public string type { get; set; }
        public Reply reply { get; set; }
    }

    public class Reply
    {
        public string id { get; set; }
        public string title { get; set; }
    }

}

