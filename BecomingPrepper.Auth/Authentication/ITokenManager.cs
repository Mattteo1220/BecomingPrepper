using Microsoft.AspNetCore.Http;

namespace BecomingPrepper.Auth.Authentication
{
    public interface ITokenManager
    {
        string Generate(string accountId, string email);
        void CreateCookie(string key, string value, HttpResponse response, bool httpOnly = true, bool isSecure = true);
        bool IsAuthorized(string token);
        string RefreshToken();
    }
}
