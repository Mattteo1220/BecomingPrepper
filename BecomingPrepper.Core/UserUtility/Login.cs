using System;
using System.Security;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using MongoDB.Driver;

namespace BecomingPrepper.Core.UserUtility
{
    public class Login : ILogin
    {
        private ISecureService _secureService;
        private ILogManager _exceptionLog;
        private IRepository<UserEntity> _userRepo;
        public string AccountId { get; set; }
        public Login(IRepository<UserEntity> userRepo, ISecureService secureService, ILogManager exceptionLog)
        {
            _userRepo = userRepo;
            _secureService = secureService;
            _exceptionLog = exceptionLog;
        }

        public bool Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            var filter = Builders<UserEntity>.Filter.Eq(p => p.Account.Username, username);
            UserEntity userEntity = null;
            try
            {
                userEntity = _userRepo.Get(filter);
            }
            catch
            {
                throw;
            }

            if (userEntity == null)
            {
                return false;
            }

            var result = _secureService.Validate(userEntity.Account.Password, password);

            if (result.NeedsUpgrade)
            {
                _exceptionLog.LogWarning(new SecurityException($"'Needs upgrading' returned true for user {username}"));
            }

            _exceptionLog.LogInformation($"User '{userEntity.Account.AccountId}' has been verified");

            AccountId = userEntity.Account.AccountId;
            return result.Verified;
        }

        public bool IsAuthenticated(string accountId)
        {
            var filter = Builders<UserEntity>.Filter.Where(u => u.Account.AccountId == accountId);
            return _userRepo.Get(filter) != null;
        }
    }
}
