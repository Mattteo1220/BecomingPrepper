using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BecomingPrepper.Api.Authentication
{
    public class TokenManager : ITokenManager
    {
        private readonly TokenInfo _tokenInfo;
        private readonly IRepository<UserEntity> _userRepo;
        public string AccountIdUsedForAuthorization { get; set; }
        public TokenManager(TokenInfo tokenInfo, IRepository<UserEntity> userRepository)
        {
            _tokenInfo = tokenInfo ?? throw new ArgumentNullException(nameof(tokenInfo));
            _userRepo = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
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
            var secret = Environment.GetEnvironmentVariable("Secret");
            var handler = new JwtSecurityTokenHandler();
            var tokenInformation = handler.ReadJwtToken(token);

            AccountIdUsedForAuthorization = tokenInformation.Claims.First(claim => claim.Type == "sub").Value;
            var filter = Builders<UserEntity>.Filter.Where(u => u.Account.AccountId == AccountIdUsedForAuthorization);
            return _userRepo.Get(filter) != null;
        }

        public void CreateCookie(string token, string accountId, HttpResponse response, bool httpOnly = true, bool isSecure = true)
        {
            var options = new CookieOptions();
            options.HttpOnly = httpOnly;
            options.Secure = isSecure;
            options.MaxAge = TimeSpan.FromMinutes(240);
            response.Cookies.Append(accountId, token, options);
        }

        public void CreateCookie(string accountId, HttpResponse response, bool httpOnly = true, bool isSecure = true)
        {
            var options = new CookieOptions();
            options.HttpOnly = httpOnly;
            options.Secure = isSecure;
            options.MaxAge = TimeSpan.FromMinutes(240);
            response.Cookies.Append("AccountId", accountId, options);
        }
    }
}
