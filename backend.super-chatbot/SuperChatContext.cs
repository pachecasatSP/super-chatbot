using backend.super_chatbot.Entidades;
using Microsoft.EntityFrameworkCore;

namespace backend.super_chatbot
{
    public class SuperChatContext : DbContext
    {
        public SuperChatContext(DbContextOptions options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Chat> Chats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {               
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Contacts).WithOne(x => x.Client).HasForeignKey(x => x.ClientId);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.ChatHistory).WithOne(x => x.Contact).HasForeignKey(x => x.ContactId);
            });
        }
    }
}
