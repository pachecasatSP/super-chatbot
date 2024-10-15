using backend.super_chatbot.Entidades;
using Microsoft.EntityFrameworkCore;

namespace backend.super_chatbot
{
    public class SuperChatContext : DbContext
    {
        public SuperChatContext(DbContextOptions options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
    }
}
