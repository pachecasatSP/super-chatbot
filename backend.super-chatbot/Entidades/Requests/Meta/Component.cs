using System.Reflection.Metadata;

namespace backend.super_chatbot.Entidades.Requests.Meta;

public abstract class Component
{
    protected Component(string type)
    {
        Type = type;
    }

    public string Type { get; set; }
    
}

public class BodyComponent : Component
{
    public BodyComponent()
        : base("body")

    {
            
    }
    public Parameter[] Parameters { get; set; }
}

public class HeaderComponent : Component
{
    public HeaderComponent()
        : base("header")

    {

    }
    public Parameter[] Parameters { get; set; }
}

public class ButtonComponent : Component
{
    public ButtonComponent() :
        base("button")
    { }

    public string Index { get;set; }
    public string Sub_type { get; set; }
    public ButtonParameters[] Parameters { get; set; }

}

public abstract class Parameter
{
    protected Parameter(string type)
    {
        Type = type;
    }
    public string Type { get; private set; }
}

public class TextParameter : Parameter
{
    public TextParameter()
        : base("text") { }

    public string? Text { get; set; }
}


public class ButtonParameters
{
    public string Type { get; set; }
    public string Payload { get; set; }
    public string Text { get;set;}

}