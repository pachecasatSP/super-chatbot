
namespace backend.super_chatbot.Entidades
{
    public class Contact
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public ICollection<Chat> ChatHistory { get; set; } = new List<Chat>();  

        internal void SetChat(Chat chat)
        {
            ChatHistory.Add(chat);
        }
    }
}
