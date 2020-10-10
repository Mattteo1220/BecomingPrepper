namespace BecomingPrepper.Api.Authentication
{
    public class TokenInfo
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryInMinutes { get; set; }
    }
}
