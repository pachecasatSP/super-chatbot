namespace backend.super_chatbot.Entidades
{
    public class Chat
    {
        public int Id { get; set; }
        public string MetaMessageId { get; set; }   
        public DateTime CreatedDate { get; set; }
        public string? VerificationCode { get; set; }
        public DateTime? VerificationCodeExpiration { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
