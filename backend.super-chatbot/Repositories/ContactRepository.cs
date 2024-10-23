using backend.super_chatbot.Entidades;
using Microsoft.EntityFrameworkCore;

namespace backend.super_chatbot.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private SuperChatContext _context;

        public ContactRepository(SuperChatContext context)
        {
            _context = context; 
        }

        public async Task<Contact> GetByPhoneNumber(string phoneNumber)
        {
            return await _context.Contacts.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        }
    }
}
