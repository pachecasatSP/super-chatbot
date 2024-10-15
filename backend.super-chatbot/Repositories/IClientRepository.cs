﻿using backend.super_chatbot.Entidades;

namespace backend.super_chatbot.Repositories
{
    public interface IClientRepository
    {
        Task<Client> CreateClient(Client entity);
        Task<Client> Get(int Id);
        Task<Client> GetByPhoneNumber(string display_phone_number);
    }
}