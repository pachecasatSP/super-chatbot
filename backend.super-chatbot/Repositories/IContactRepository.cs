using backend.super_chatbot.Entidades;

namespace backend.super_chatbot.Repositories
{
    public interface IContactRepository
    {
        Task<Contact> GetByPhoneNumber(string phoneNumber);
    }
}
