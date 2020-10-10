using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BecomingPrepper.Api.Authentication
{
    public class TokenManager : ITokenManager
    {
        private TokenInfo _tokenInfo;
        public TokenManager(TokenInfo tokenInfo)
        {
            _tokenInfo = tokenInfo ?? throw new ArgumentNullException(nameof(tokenInfo));
        }
        public string Generate(string accountId, string email)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Sub,  accountId)
            };

            JwtSecurityToken token = new TokenBuilder()
                .AddAudience(_tokenInfo.Audience)
                .AddIssuer(_tokenInfo.Issuer)
                .AddExpiry(_tokenInfo.ExpiryInMinutes)
                .AddKey(Environment.GetEnvironmentVariable("Secret"))
                .AddClaims(claims)
                .Build();

            string accessToken = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return $"Bearer {accessToken}";
        }

        public void CreateCookie(string token, HttpResponse response)
        {
            var options = new CookieOptions();
            options.HttpOnly = true;
            options.MaxAge = TimeSpan.FromMinutes(240);
            response.Cookies.Append("Token", token, options);
        }
    }
}
