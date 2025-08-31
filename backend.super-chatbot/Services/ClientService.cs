using backend.super_chatbot.Entidades;
using backend.super_chatbot.Entidades.Requests;
using backend.super_chatbot.Repositories;
using backend.super_chatbot.Services.ClientMeta;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace backend.super_chatbot.Services
{
    public class ClientService : IClientService
    {
        private IClientRepository _repository;
        private IClientMeta _clientMeta;

        public ClientService(IClientRepository repository,
                             IClientMeta clientMeta)
        {
            _repository = repository;
            _clientMeta = clientMeta;
        }

        public async Task<CreateClientResponse> CreateClient(CreateClientRequest request)
        {
            var client = await _repository.Create(new Client()
            {
                MetaPhoneId = request.Meta_Tel_Id,
                Name = request.Nome,
                PhoneNumber = request.NumeroTelefonico,
                TokenOnMeta = request.Meta_Token,
                FriendlyName = request.Nome,
                Flow = new Flow()
                {
                    Steps = [new Step() { WaitWebhook = false, NextStep =2, Type = "greeting",Content = new Content(){
                      Type = "text",ContentValue="{                     \"text\": {                         \"body\": \"Olá {{ProfileName}}! \\r\\n Seja bem-vindo a {{ClientName}}!\\r\\n Vamos iniciar seu atendimento. \"                     }                 }"} },
                    new Step(){

                        WaitWebhook = true,
                        NextStep = 0,
                        Type = "menu", Content = new Content(){
                            Type = "interactive",
                            ContentValue = @" {
                    ""Type"": ""button"",
                    ""Header"": {
                        ""Type"": ""image"",
                        ""image"": {
                            ""Id"": ""851310220533586""
                        }
                    },
                    ""Body"": {
                        ""Text"": ""Para iniciar, selecione abaixo uma das opções disponíveis.\r\n Caso a opção desejada não seja mostrada. .\r\nEscreve objetivamente o que precisa.""
                    },
                    ""Action"": {
                        ""Buttons"": [
                            {
                                ""Type"": ""reply"",
                                ""Reply"": {
                                    ""Id"": ""quero-incluir-agenda"",
                                    ""Title"": ""Quero Agendar um Serviço""
                                }
                            },
                            {
                                ""Type"": ""reply"",
                                ""Reply"": {
                                    ""Id"": ""quero-consultar-um-agendamento"",
                                    ""Title"": ""Quero consultar um agendamento""
                                }
                            },
                            {
                                ""Type"": ""reply"",
                                ""Reply"": {
                                    ""Id"": ""quero-encerrar-atendimento"",
                                    ""Title"": ""Encerrar atendimento""
                                }
                            }
                        ]
                    }
                }
            }"
                        }
                    }]
                }
            });

            var result = new CreateClientResponse()
            {
                Token = CreateToken(client),
            };

            return result;
        }

        public async Task<CreateClientResponse> ExchangeToken(string authCode)
        {
            var response = await _clientMeta.ExchangeCodeByToken(authCode);

            var result = new CreateClientResponse()
            {
                Token = response.Access_token,
            };

            return result;
        }

        public async Task RegisterAccount(int clientId, string phoneNumberId)
        {
            var client = await _repository.Get(clientId);
            var response = await _clientMeta.RegisterAccount(client, phoneNumberId);
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
