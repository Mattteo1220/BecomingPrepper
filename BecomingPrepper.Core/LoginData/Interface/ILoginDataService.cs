﻿using BecomingPrepper.Data.Entities.Logins;

namespace BecomingPrepper.Core.TokenService.Interface
{
    public interface ILoginDataService
    {
        Login FetchLastLoginData(string accountId);
        void CreateLoginData(Login login);
        void UpdateToken(string accountId, string token);
    }
}
