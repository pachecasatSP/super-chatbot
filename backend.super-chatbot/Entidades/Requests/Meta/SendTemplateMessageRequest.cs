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
        

}

