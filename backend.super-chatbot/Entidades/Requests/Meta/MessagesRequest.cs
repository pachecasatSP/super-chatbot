
namespace backend.super_chatbot.Entidades.Requests.Meta;

public class MessagesRequest
{
    public Entry[] Entry { get; set; }

    public string GetSenderPhoneNumber() =>
        Entry[0].Changes[0].Value.Metadata?.Display_phone_number!;

    internal Button GetButton() =>
        Entry[0].Changes[0].Value.Messages[0]?.Button!;

    internal Message GetMessage() =>
        Entry[0].Changes[0].Value.Messages?[0]!;

    internal Text GetText() =>
       Entry[0].Changes[0].Value.Messages[0].Text!;

    internal Document GetDocument() =>
       Entry[0].Changes[0].Value.Messages[0].Document;

    internal MessageStatus[] GetStatuses() =>
        Entry[0].Changes[0].Value?.Statuses!;

    internal string GetFrom() =>
        Entry[0].Changes[0].Value.Messages?[0].From!;

    internal Contact GetContact() =>
        Entry[0].Changes[0].Value.Contacts?[0]!;

    internal InteractiveResponse GetInteractiveResponse() =>
        Entry[0].Changes[0].Value?.Messages?[0].Interactive!;
}
