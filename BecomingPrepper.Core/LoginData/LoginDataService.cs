using System;
using BecomingPrepper.Core.TokenService.Interface;
using BecomingPrepper.Data.Entities.Logins;
using BecomingPrepper.Data.Interfaces;
using MongoDB.Driver;

namespace BecomingPrepper.Core.TokenService
{
    public class LoginDataService : ILoginDataService
    {
        private readonly IRepository<Login> _loginRepository;
        public LoginDataService(IRepository<Login> loginRepository)
        {
            _loginRepository = loginRepository ?? throw new ArgumentNullException(nameof(loginRepository));
        }
        public Login FetchLastLoginData(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            var filter = Builders<Login>.Filter.Where(l => l.AccountId == accountId);
            return _loginRepository.Get(filter);
        }

        public void CreateLoginData(Login login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            _loginRepository.Add(login);
        }
    }
}
