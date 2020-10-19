﻿using Microsoft.AspNetCore.Http;

namespace BecomingPrepper.Api.Authentication
{
    public interface ITokenManager
    {
        string Generate(string accountId, string email);
        void CreateCookie(string token, string accountId, HttpResponse response, bool httpOnly = true, bool isSecure = true);
        void CreateCookie(string accountId, HttpResponse response, bool httpOnly = true, bool isSecure = true);
        bool IsAuthorized(string token);
        string AccountIdUsedForAuthorization { get; set; }
    }
}