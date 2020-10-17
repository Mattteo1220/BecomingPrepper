using Microsoft.AspNetCore.Http;

namespace BecomingPrepper.Api.Authentication
{
    public interface ITokenManager
    {
        string Generate(string accountId, string email);
        void CreateCookie(string token, HttpResponse response);
        bool Authorize(string token);
        string AccountIdUsedForAuthorization { get; set; }
    }
}
