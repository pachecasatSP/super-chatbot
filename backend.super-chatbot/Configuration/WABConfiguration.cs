namespace backend.super_chatbot.Configuration
{
    public class WABConfiguration
    {
        public const string WABOptions = "WABOptions";

        public string? BaseAddress { get; set; }
        public string? BearerToken { get; set; }
        public string? MessagesEndpoint { get; set; }
        public string? ExchangeTokenEndpoint { get; set; }
        public string? Client_secret { get; set; }
        public string? Client_id { get; set; }
        public string? Redirect_uri { get; set; }
    }
}
