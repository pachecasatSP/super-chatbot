using backend.super_chatbot.Entidades;
using backend.super_chatbot.Entidades.Requests;
using backend.super_chatbot.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace backend.super_chatbot.Services
{
    public class ClientService : IClientService
    {
        public ClientService(IClientRepository repository)
        {
            Repository = repository;
        }

        public IClientRepository Repository { get; }

        public async Task<CreateClientResponse> CreateClient(CreateClientRequest request)
        {
            var client = await Repository.Create(new Client()
            {
                MetaPhoneId = request.Meta_Tel_Id,
                Name = request.Nome,
                PhoneNumber = request.NumeroTelefonico
            });

            var result = new CreateClientResponse()
            {                
                Token = CreateToken(client),
            };

            return result;
        }

        private string CreateToken(Client client)
        {
            var claims = new[]
           {
                new Claim(type: JwtRegisteredClaimNames.Sub, client.Id.ToString()),
                new Claim(type: JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(type: ClaimTypes.NameIdentifier, client.Name!.ToString()),
                new Claim(type: ClaimTypes.MobilePhone, client.PhoneNumber),
                new Claim(type: "metaphoneid", client.MetaPhoneId),
                new Claim(type: "clientId", client.Id.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMonths(1)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
