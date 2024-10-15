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

        public async Task<Client> Get(int Id)
        {
            return await Context.Clients.FirstOrDefaultAsync(x => x.Id == Id);
        }
    }
}
