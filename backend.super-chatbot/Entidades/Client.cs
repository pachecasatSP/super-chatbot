

namespace backend.super_chatbot.Entidades
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string PhoneNumber { get; set; }
        public string MetaPhoneId { get; set; }
        public string TokenOnMeta { get; set; } 
        public string WebHookToken { get; set; }

        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

        internal void SetContact(Contact contact)
        {
            Contacts.Add(contact);
        }
    }
}
