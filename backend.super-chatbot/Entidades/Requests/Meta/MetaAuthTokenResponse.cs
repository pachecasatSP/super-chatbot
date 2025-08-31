using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace backend.super_chatbot.Entidades.Requests.Meta
{
    public class MetaAuthTokenResponse
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
    }
}

