using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using BecomingPrepper.Api.Authentication;
using Microsoft.AspNetCore.Http;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BecomingPrepper.Auth.Authentication
{
    public class TokenManager : ITokenManager
    {
        private readonly TokenInfo _tokenInfo;
        private string _accountId;
        private string _email;
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

            return accessToken;
        }

        public bool IsAuthorized(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenInformation = handler.ReadJwtToken(token);

            var isValidToValid = tokenInformation.ValidTo.ToLocalTime() >= DateTime.Now;
            var isIssuerValid = tokenInformation.Claims.Any(c => c.Issuer == _tokenInfo.Issuer);
            var isAudienceValid = tokenInformation.Audiences.Any(a => a == _tokenInfo.Audience);

            _accountId = tokenInformation.Claims.First(claim => claim.Type == "sub").Value;
            _email = tokenInformation.Claims.First(claim => claim.Type == "email").Value;
            return (isValidToValid && isIssuerValid && isAudienceValid);
        }

        public string RefreshToken()
        {
            return Generate(_accountId, _email);
        }

        public void CreateCookie(string key, string value, HttpResponse response, bool httpOnly = true, bool isSecure = true)
        {
            var options = new CookieOptions();
            options.HttpOnly = httpOnly;
            options.Secure = isSecure;
            options.MaxAge = TimeSpan.FromMinutes(240);
            response.Cookies.Append(key, value, options);
        }
    }
}
