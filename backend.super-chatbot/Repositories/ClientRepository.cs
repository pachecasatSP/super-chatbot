using backend.super_chatbot.Entidades;
using Microsoft.EntityFrameworkCore;

namespace backend.super_chatbot.Repositories
{
    public class ClientRepository : IClientRepository
    {
        public ClientRepository(SuperChatContext context)
        {
            Context = context;
        }

        public SuperChatContext Context { get; }

        public async Task<Client> CreateClient(Client entity)
        {
            Context.Clients.Add(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<Client> Get(int Id) =>
            await Context.Clients.FirstOrDefaultAsync(x => x.Id == Id);
               
        public async Task<Client> GetByPhoneNumber(string display_phone_number) => 
            await Context.Clients.FirstOrDefaultAsync(x => x.PhoneNumber == display_phone_number);
    }
}
